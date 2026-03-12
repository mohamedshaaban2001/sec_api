using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecControlList;
using MapsterMapper;
using LoggerService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Repositories.Models;

public class SecControlListRepository : RepositoryBase<SecControlList, SecControlListDto, SecControlListCreateDto, SecControlListUpdateDto>, ISecControlListRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecControlListRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger,repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    private static readonly Regex ControlCodeRegex = new("^[A-Za-z0-9_\\-\\s]+$", RegexOptions.Compiled);

    public override async Task<ParentResponseModel> FindAll()
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecControlLists
                .Where(e => e.IsDeleted == false)
                .AsNoTracking()
                .Select(e => new SecControlListDto
                {
                    Id = e.Id,
                    ControlCode = e.ControlCode,
                    ControlDescription = e.ControlDescription
                })
                .OrderBy(e => e.ControlCode)
                .ToListAsync();

            return new ListOfObjectsResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = listOfObjects
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecControlList).Name} ===> FindAll ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public override async Task<ParentResponseModel> Create(SecControlListCreateDto entityCreate)
    {
        try
        {
            var normalizedControlCode = entityCreate.ControlCode?.Trim();
            var normalizedDescription = entityCreate.ControlDescription?.Trim();

            var validationResponse = ValidateControlPayload(normalizedControlCode, normalizedDescription);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var isDuplicate = await RepositoryContext.SecControlLists
                .AnyAsync(e => e.IsDeleted == false && e.ControlCode.ToLower() == normalizedControlCode!.ToLower());
            if (isDuplicate)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.VioleteConstrains,
                    IsDone = false,
                    ReturnMessage = "Control code already exists."
                };
            }

            var fallbackPageId = await ResolveFallbackPageId();
            if (fallbackPageId == null)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.ObjectNotFound,
                    IsDone = false,
                    ReturnMessage = "No page found to attach control."
                };
            }

            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var entity = new SecControlList
            {
                ControlCode = normalizedControlCode!,
                ControlDescription = normalizedDescription!,
                PageId = fallbackPageId.Value,
                InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                InsertDate = DateTime.Now,
                IsDeleted = false
            };

            await RepositoryContext.SecControlLists.AddAsync(entity);
            await RepositoryContext.SaveChangesAsync();

            return new SingleObjectResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Object Added Successufly",
                SingleObject = new SecControlListDto
                {
                    Id = entity.Id,
                    ControlCode = entity.ControlCode,
                    ControlDescription = entity.ControlDescription
                }
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecControlList).Name} ===> Create ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public override async Task<ParentResponseModel> Update(SecControlListUpdateDto entityUpdate)
    {
        try
        {
            var normalizedControlCode = entityUpdate.ControlCode?.Trim();
            var normalizedDescription = entityUpdate.ControlDescription?.Trim();

            var validationResponse = ValidateControlPayload(normalizedControlCode, normalizedDescription);
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var entity = await RepositoryContext.SecControlLists
                .FirstOrDefaultAsync(e => e.Id == entityUpdate.Id && e.IsDeleted == false);
            if (entity == null)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.ObjectNotFound,
                    IsDone = false,
                    ReturnMessage = "Object not found"
                };
            }

            var isDuplicate = await RepositoryContext.SecControlLists
                .AnyAsync(e => e.IsDeleted == false &&
                               e.Id != entityUpdate.Id &&
                               e.ControlCode.ToLower() == normalizedControlCode!.ToLower());
            if (isDuplicate)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.VioleteConstrains,
                    IsDone = false,
                    ReturnMessage = "Control code already exists."
                };
            }

            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            entity.ControlCode = normalizedControlCode!;
            entity.ControlDescription = normalizedDescription!;
            entity.UpdateUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no update user code detected";
            entity.LastUpdate = DateTime.Now;

            await RepositoryContext.SaveChangesAsync();
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Object updated successufly",
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecControlList).Name} ===> Update ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async override Task<ParentResponseModel> Delete(int id, bool softDelete = true)
    {
        try
        {
            var entity = await RepositoryContext.SecControlLists.Include(e => e.SecGroupControls)
                .FirstOrDefaultAsync(t => t.Id == id);
            string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            if (entity != null)
            {
                if (entity.SecGroupControls.Count > 0)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.VioleteConstrains,
                        IsDone = false,
                        ReturnMessage = "object has child object , Please Delete GroupControls"
                    };
                }
                entity.IsDeleted = true;
                entity.DeleteUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no delete user code detected";
                entity.DeleteDate = DateTime.Now;
                await RepositoryContext.SaveChangesAsync();
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.noError,
                    IsDone = true,
                    ReturnMessage = "object removed successufly"
                };
            }
            else
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.ObjectNotFound,
                    IsDone = false,
                    ReturnMessage = "no object found with that id"
                };
            }
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecControlList).Name} ===> Delete ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    private ParentResponseModel? ValidateControlPayload(string? controlCode, string? controlDescription)
    {
        if (string.IsNullOrWhiteSpace(controlCode))
        {
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.missingValues,
                IsDone = false,
                ReturnMessage = "Control code is required."
            };
        }

        if (!ControlCodeRegex.IsMatch(controlCode))
        {
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.missingValues,
                IsDone = false,
                ReturnMessage = "Control code must be in English letters, numbers, space, underscore, or dash."
            };
        }

        if (string.IsNullOrWhiteSpace(controlDescription))
        {
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.missingValues,
                IsDone = false,
                ReturnMessage = "Control description is required."
            };
        }

        return null;
    }

    private async Task<int?> ResolveFallbackPageId()
    {
        var pageId = await RepositoryContext.SecPages
            .Where(p => p.IsDeleted == false)
            .OrderBy(p => p.Id)
            .Select(p => (int?)p.Id)
            .FirstOrDefaultAsync();

        if (pageId.HasValue)
        {
            return pageId.Value;
        }

        return await RepositoryContext.SecControlLists
            .Where(c => c.IsDeleted == false)
            .OrderBy(c => c.Id)
            .Select(c => (int?)c.PageId)
            .FirstOrDefaultAsync();
    }
}
