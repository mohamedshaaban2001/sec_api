using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecModule;
using MapsterMapper;
using Grpc_Client;
using LoggerService;
using Contracts.DTOs.SecService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System.Text.RegularExpressions;
using Contracts.DTOs.SecGroupPage;

namespace Repositories.Models;

public class SecModuleRepository : RepositoryBase<SecModule, SecModuleDto, SecModuleCreateDto, SecModuleUpdateDto>, ISecModuleRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecModuleRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger,repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<ParentResponseModel> GetServicesBasedOnModule(int ModuleId)
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecServices.Where(e=>e.ModuleNo==ModuleId).AsNoTracking().ProjectToType<SecServiceDto>().ToListAsync();
            return new ListOfObjectsResponseModel<SecServiceDto>()
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

    public  async Task<ParentResponseModel> CreateServicesForModules(AssignServiceForModule assignServiceForModule)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var serviceNameExist= await RepositoryContext.SecServices.AnyAsync(e=>e.ServiceName.Equals(assignServiceForModule.ServiceName));
            if(serviceNameExist)
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.DataBaseFauiler,
                    IsDone = false,
                    ReturnMessage = "Name Is Exist Already"
                };

            var entity =new SecService
            {
                InsertUserCode= !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                InsertDate = DateTime.Now,
                ModuleNo = assignServiceForModule.ModuleCode,
                ServiceName = assignServiceForModule.ServiceName,
            };
            await RepositoryContext.SecServices.AddAsync(entity);
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<SecServiceDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                SingleObject = entity.Adapt<SecServiceDto>()
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

    public async override Task<ParentResponseModel> Delete(int id, bool softDelete = true)
    {
        try
        {
            var entity = await RepositoryContext.SecModules
                .Include(e => e.SecModuleGroups).Include(e => e.SecPages).Include(e => e.SecServices)
                .FirstOrDefaultAsync(t => t.Id == id);
            string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            if (entity != null)
            {
                if (entity.SecModuleGroups.Count > 0 ||  entity.SecPages.Count > 0 || entity.SecServices.Count > 0)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.VioleteConstrains,
                        IsDone = false,
                        ReturnMessage = "object has child object ModuleGroups||Pages||Services"
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
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> Delete ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> GetModulesWithTakenServices()
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecModules.AsNoTracking().
                Select(e => new SecModulesWithTakenAction
                {
                    Id = e.Id,
                    ModuleName=e.ModuleName,
                    ModuleIcon=e.Icon,
                    IsModuleAssigned=e.SecServices.Any(e=>(bool)e.IsTaken),
                    ModuleColor=e.Color,
                    serviceTakens = e.SecServices.Select(e => new ServiceTaken
                    {
                        Id = e.Id,
                        ServiceName = e.ServiceName,
                        IsServiceTaken = e.IsTaken

                    }).ToList()
                }).ToListAsync();


            return new ListOfObjectsResponseModel<SecModulesWithTakenAction>()
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

    public async Task<ParentResponseModel> AssignTakenToServiceInModule(AssignTakenServiceForModule assignTakenServiceForModule)
    {
        try
        {
          
            var entity = await RepositoryContext.SecModules.Include(e=>e.SecServices).FirstOrDefaultAsync(e => e.Id == assignTakenServiceForModule.ModuleId);
           
            if (!assignTakenServiceForModule.ModuleAssigned)
            {
                entity.IsTaken = false;
            }else
            {
                entity.IsTaken = true;
            }
            foreach (var item in entity.SecServices)
            {
                if (assignTakenServiceForModule.ServiceTakens!=null && assignTakenServiceForModule.ServiceTakens.Contains(item.Id))
                {
                    item.IsTaken = true;
                }
                else
                {
                    item.IsTaken = false;
                }       
            }
            await RepositoryContext.SaveChangesAsync();         
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Assign Taken To Service In Module Successufly"
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> AssignTakenToServiceInModule ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }


}
