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
                PageUrl=e.PageUrl,
                PageOrder = e.PageOrder,
                ModuleCode = e.ModuleCode,
                ModuleName = e.SecModule.ModuleName,
                ServiceCode = e.ServiceCode,
                ServiceName = e.SecService.ServiceName,
                //Groups = e.SecGroupPages.Select(e => new SecGroupPageSmallDto
                //{
                //    Id = e.Id,
                //    GroupName = e.SecGroup.GroupName
                //}).ToList(),
                Controls=e.SecControlLists.Select(e=> new Contracts.DTOs.SecControlList.SecControlListDto
                {
                    Id=e.Id,
                    PageId=e.PageId,
                    ControlCode=e.ControlCode,
                    ControlDescription=e.ControlDescription
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
                Id=e.Id,
                Name=e.GroupName,
                ModuleIds=e.SecModuleGroups.Select(e=>e.ModuleCode).ToList()    
            }).ToListAsync();
            var modules = await RepositoryContext.SecModules.Where(e=>(bool)e.IsTaken).Select(e => new SecModuleSmallDto
            {
                Id = e.Id,
                Name = e.ModuleName,
                Color=e.Color,
                Icon=e.Icon
            }).ToListAsync();
            var services = await RepositoryContext.SecServices.Where(e => (bool)e.IsTaken).Select(e => new SecServiceDto
            {
                Id = e.Id,
                ModuleNo=e.ModuleNo,
                ServiceName=e.ServiceName
            }).ToListAsync();
            lookups.Modules = modules;
            lookups.Groups=groups;
            lookups.Services=services;
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
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var entity = new SecPage()
            {
                InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                InsertDate = DateTime.Now,
                PageName = entityCreate.PageName,
                PageOrder = entityCreate.PageOrder,
                ParentId = entityCreate.ParentId,
                PageUrl = entityCreate.PageUrl,
                ModuleCode = entityCreate.ModuleCode,
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
            entity.PageOrder = secPageUpdateDto.PageOrder;
            entity.ParentId = secPageUpdateDto.ParentId;

            entity.Icon = secPageUpdateDto.Icon ?? entity.Icon;
            entity.PageUrl = secPageUpdateDto.PageUrl ?? entity.PageUrl;
            entity.ModuleCode = secPageUpdateDto.ModuleCode;
            
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
}
