using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecPage;
using MapsterMapper;
using LoggerService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecModule;
using Mapster;
using Contracts.DTOs.Privilege;
using Contracts.DTOs.SecControlList;
using Contracts.DTOs.SecService;

namespace Repositories.Models;

public class SecPageRepository : RepositoryBase<SecPage, SecPageDto, SecPageCreateDto, SecPageUpdateDto>, ISecPageRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecPageRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<ParentResponseModel> FindAll()
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecPages.AsNoTracking().Select(e => new SecPageDto
            {
                Id = e.Id,
                PageName = e.PageName,
                Icon = e.Icon,
                ParentId = e.ParentId,
                ParentName = e.Parent.PageName,
                PageUrl = e.PageUrl,
                PageOrder = e.PageOrder,
                ServiceCode = e.ServiceCode,
                ServiceName = e.SecService.ServiceName,
                Privileges = e.SecControlLists.Where(c => c.IsDeleted == false).Select(c => new PrivilegeDto
                {
                    Id = c.Id,
                    PageId = c.PageId,
                    Code = c.ControlCode,
                    Description = c.ControlDescription
                }).ToList()
            }).ToListAsync();
            return new ListOfObjectsResponseModel<SecPageDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = listOfObjects
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> GetServicesBasedOnModule ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }
    public  async Task<ParentResponseModel> GetPagesForLookup()
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecPages.AsNoTracking().Select(e => new SecPageAutoCompleteDto
            {
                Id= e.Id,
                PageName = e.PageName,
                Icon = e.Icon,
            }).ToListAsync();

            return new ListOfObjectsResponseModel<SecPageAutoCompleteDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = listOfObjects
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> GetPagesForLookup ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> GetLookupsForCreatePage()
    {
        try
        {
            var lookups = new SecPagelookupDto();
            var groups = await RepositoryContext.SecGroups.Select(e => new SecGroupSmallDto
            {
                Id = e.Id,
                Name = e.GroupName
            }).ToListAsync();
            var services = await RepositoryContext.SecServices.Where(e => (bool)e.IsTaken).Select(e => new SecServiceDto
            {
                Id = e.Id,
                ModuleNo = e.ModuleNo,
                ServiceName = e.ServiceName
            }).ToListAsync();
            lookups.Groups = groups;
            lookups.Services = services;
            return new SingleObjectResponseModel<SecPagelookupDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                SingleObject = lookups
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> GetServicesBasedOnModule ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }



    public async override Task<ParentResponseModel> Create(SecPageCreateDto entityCreate)
    {
        try
        {
            var resolvedModuleCode = await ResolveModuleCodeForCreate(
                entityCreate.ParentId,
                entityCreate.ServiceCode,
                entityCreate.ModuleCode
            );

            if (resolvedModuleCode == null)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.ObjectNotFound,
                    IsDone = false,
                    ReturnMessage = "No available module found for this page."
                };
            }

            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var entity = new SecPage()
            {
                InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                InsertDate = DateTime.Now,
                PageName = entityCreate.PageName,
                PageOrder = entityCreate.PageOrder,
                ParentId = entityCreate.ParentId,
                PageUrl = entityCreate.PageUrl,
                ModuleCode = resolvedModuleCode.Value,
                ServiceCode = entityCreate.ServiceCode,
                Icon = entityCreate.Icon
                //SecGroupPages = entityCreate.GroupIds.Select(group => new SecGroupPage()
                //{
                //    GroupCode = group,
                //    InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                //    InsertDate = DateTime.Now
                //}).ToList()
            };

            await RepositoryContext.SecPages.AddAsync(entity);
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<int>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Object loaded successfully",
                SingleObject = entity.Id
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> Create ");
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

            var entity = await RepositoryContext.SecPages.Include(e => e.SecGroupPages)
                .Include(e => e.SecControlLists).Include(e=>e.SecGroupControls).Include(e => e.Children).FirstOrDefaultAsync(t => t.Id == id);

            

            string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            if (entity != null)
            {
                if(entity.Children.Count > 0 ||entity.SecControlLists.Count>0||entity.SecGroupControls.Count>0||entity.SecGroupControls.Count>0)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.VioleteConstrains,
                        IsDone = false,
                        ReturnMessage = "object has child object , Please Delete Child Pages First || Assigned Controls For Page To Group  || Assigned Pages To Group ||Assigned Controls To Page"
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
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> Delete ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public override async Task<ParentResponseModel> Update(SecPageUpdateDto secPageUpdateDto)
    {
        try
        {
            var entity = await RepositoryContext.SecPages.Include(e => e.SecGroupPages).AsTracking().FirstOrDefaultAsync(t => t.Id == secPageUpdateDto.Id);

            if (entity == null)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.ObjectNotFound,
                    IsDone = false,
                    ReturnMessage = "Object not found"
                };
            }

            entity.PageName = secPageUpdateDto.PageName ?? entity.PageName;

            var newOrder = secPageUpdateDto.PageOrder;
            var oldOrder = entity.PageOrder;
            if (newOrder != oldOrder)
            {
                var other = await RepositoryContext.SecPages.AsTracking()
                    .FirstOrDefaultAsync(p =>
                        p.ModuleCode == entity.ModuleCode &&
                        p.PageOrder == newOrder &&
                        p.Id != entity.Id &&
                        p.IsDeleted == false);
                if (other != null)
                    other.PageOrder = oldOrder;
            }

            entity.PageOrder = newOrder;
            entity.ParentId = secPageUpdateDto.ParentId;

            entity.Icon = secPageUpdateDto.Icon ?? entity.Icon;
            entity.PageUrl = secPageUpdateDto.PageUrl ?? entity.PageUrl;
            if (secPageUpdateDto.ModuleCode.HasValue && secPageUpdateDto.ModuleCode.Value > 0)
            {
                entity.ModuleCode = secPageUpdateDto.ModuleCode.Value;
            }

            entity.ServiceCode = secPageUpdateDto.ServiceCode!=null?(int)secPageUpdateDto.ServiceCode:null;


            //if (secPageUpdateDto.GroupIds.Count > 0 && entity.SecGroupPages.Count > 0)
            //{
            //    RepositoryContext.SecGroupPages.RemoveRange(entity.SecGroupPages);
            //}
            //entity.SecGroupPages = secPageUpdateDto.GroupIds.Select(groupId => new SecGroupPage
            //{
            //    PageCode = secPageUpdateDto.Id,
            //    GroupCode = groupId
            //}).ToList();

            await RepositoryContext.SaveChangesAsync();
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Object updated successufly",
            };
        }
        catch (DbUpdateException ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> Update ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage =
                    "Page order conflicts with another page in the same module. Use PUT SecPage/Reorder with all new orders in one request, or ensure orders are unique per module.",
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> Update ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> ReorderPages(ReorderSecPagesDto reorder)
    {
        try
        {
            if (reorder?.Items == null || reorder.Items.Count == 0)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.missingValues,
                    IsDone = false,
                    ReturnMessage = "Items are required."
                };
            }

            var ids = reorder.Items.Select(i => i.Id).Distinct().ToList();
            var explicitOrderById = reorder.Items
                .GroupBy(i => i.Id)
                .ToDictionary(g => g.Key, g => g.Last().PageOrder);

            await using var tx = await RepositoryContext.Database.BeginTransactionAsync();
            try
            {
                var batchPages = await RepositoryContext.SecPages
                    .Where(p => ids.Contains(p.Id) && p.IsDeleted == false)
                    .ToListAsync();

                if (batchPages.Count != ids.Count)
                {
                    await tx.RollbackAsync();
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.ObjectNotFound,
                        IsDone = false,
                        ReturnMessage = "One or more pages were not found."
                    };
                }

                var moduleCodes = batchPages.Select(p => p.ModuleCode).Distinct().ToList();
                var allInModules = await RepositoryContext.SecPages
                    .Where(p => moduleCodes.Contains(p.ModuleCode) && p.IsDeleted == false)
                    .ToListAsync();

                var oldOrderById = allInModules.ToDictionary(p => p.Id, p => p.PageOrder);

                const int tempOffset = 1_000_000;
                foreach (var p in allInModules)
                    p.PageOrder = tempOffset + p.Id;

                await RepositoryContext.SaveChangesAsync();

                foreach (var moduleCode in moduleCodes)
                {
                    var inMod = allInModules.Where(p => p.ModuleCode == moduleCode).ToList();
                    var used = new HashSet<int>();
                    foreach (var p in inMod)
                    {
                        if (explicitOrderById.TryGetValue(p.Id, out var ord))
                        {
                            p.PageOrder = ord;
                            used.Add(ord);
                        }
                    }

                    var orphans = inMod
                        .Where(p => !explicitOrderById.ContainsKey(p.Id))
                        .OrderBy(p => oldOrderById[p.Id])
                        .ToList();
                    var next = 1;
                    foreach (var o in orphans)
                    {
                        while (used.Contains(next))
                            next++;
                        o.PageOrder = next;
                        used.Add(next);
                        next++;
                    }
                }

                await RepositoryContext.SaveChangesAsync();
                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }

            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Page order updated successfully."
            };
        }
        catch (DbUpdateException ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> ReorderPages ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage =
                    "Could not save page order. Ensure each page order is unique within its module, or check for duplicate ids in the request.",
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecPage).Name} ===> ReorderPages ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> CreateControlForPage(AddControlToPage assignControlToPage)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var ControlNameExist = await RepositoryContext.SecControlLists.AnyAsync(e => e.ControlCode.Equals(assignControlToPage.ControlCode));
            if (ControlNameExist)
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = "Control Code Is Exist Already"
                };
            var entity = new SecControlList
            {
                InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                InsertDate = DateTime.Now,
                ControlCode = assignControlToPage.ControlCode,
                ControlDescription = assignControlToPage.ControlName,
                PageId=assignControlToPage.PageId
            };
            await RepositoryContext.SecControlLists.AddAsync(entity);
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                SingleObject = entity.Adapt<SecControlListDto>()
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> Create ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    private async Task<int?> ResolveModuleCodeForCreate(int? parentId, int? serviceCode, int? requestedModuleCode)
    {
        if (requestedModuleCode.HasValue && requestedModuleCode.Value > 0)
        {
            return requestedModuleCode.Value;
        }

        if (parentId.HasValue)
        {
            var parentModuleCode = await RepositoryContext.SecPages
                .AsNoTracking()
                .Where(p => p.Id == parentId.Value)
                .Select(p => (int?)p.ModuleCode)
                .FirstOrDefaultAsync();

            if (parentModuleCode.HasValue && parentModuleCode.Value > 0)
            {
                return parentModuleCode.Value;
            }
        }

        if (serviceCode.HasValue)
        {
            var serviceModuleCode = await RepositoryContext.SecServices
                .AsNoTracking()
                .Where(s => s.Id == serviceCode.Value)
                .Select(s => (int?)s.ModuleNo)
                .FirstOrDefaultAsync();

            if (serviceModuleCode.HasValue && serviceModuleCode.Value > 0)
            {
                return serviceModuleCode.Value;
            }
        }

        return await RepositoryContext.SecModules
            .AsNoTracking()
            .Where(m => m.IsDeleted == false && m.IsTaken == true)
            .OrderBy(m => m.Id)
            .Select(m => (int?)m.Id)
            .FirstOrDefaultAsync();
    }
}
