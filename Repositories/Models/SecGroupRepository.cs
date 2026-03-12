using Contracts.interfaces.Models;
using Entities.Models.Tables;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Contracts.DTOs.SecGroup;
using MapsterMapper;
using LoggerService;
using Contracts.enums;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Contracts.DTOs.SecGroupPage;
using Contracts.DTOs.SecControlList;
using Contracts.DTOs.SecModule;

namespace Repositories.Models;

public class SecGroupRepository : RepositoryBase<SecGroup, SecGroupDto, SecGroupCreateDto, SecGroupUpdateDto>, ISecGroupRepository
{
    private readonly RepositoryContext _repositoryContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    //private readonly IStructureGrpcClient _structureGrpcClient;





    public SecGroupRepository(RepositoryContext repositoryContext, IHttpContextAccessor httpContextAccessor
        , IMapper mapper, ILoggerManager logger)
        : base(logger, repositoryContext, httpContextAccessor, mapper)
    {
        _repositoryContext = repositoryContext;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _logger = logger;
    }

    public async override Task<ParentResponseModel> FindAll()
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecGroups
                .AsNoTracking()
                .Select(e => new { e.Id, e.GroupName })
                .ToListAsync();

            var headers = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string> { { "field", "groupName" }, { "header", "اسم المجموعة" } }
        };

            var rows = listOfObjects.Select(group =>
            {
                var row = new Dictionary<string, object>
            {
                { "groupName", group.GroupName },
                { "groupId", group.Id }
            };

                return row;
            }).ToList();

            var final = new SecGroupDto()
            {
                Headers = headers,
                Rows = rows,
            };

            return new SingleObjectResponseModel<SecGroupDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successfully",
                SingleObject = final
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> FindAll");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }
    public async override Task<ParentResponseModel> Create(SecGroupCreateDto entityCreate)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            var entity = new SecGroup()
            {
                InsertUserCode = !string.IsNullOrEmpty(userCode) ? userCode : "no create user code detected",
                InsertDate = DateTime.Now,
                GroupName = entityCreate.GroupName
            };

            await RepositoryContext.SecGroups.AddAsync(entity);
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
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> Create ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> AvailableModulesWithGroup(int groupId)
    {
        try
        {

            var availablesModules = await RepositoryContext.SecModules.AsNoTracking().Where(e=>(bool)e.IsTaken)
                .Select(e => new AvailableModulesWithGroup
                {
                    ModuleId = e.Id,
                    ModuleName = e.ModuleName,
                    IsAssigned = e.SecModuleGroups.Any(e => e.GroupCode == groupId)
                }).ToListAsync();

            return new ListOfObjectsResponseModel<AvailableModulesWithGroup>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = availablesModules
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> AvailableModulesWithGroup ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public override async Task<ParentResponseModel> Update(SecGroupUpdateDto secGroupUpdateDto)
    {
        try
        {
            var entity = await RepositoryContext.SecGroups.AsTracking().FirstOrDefaultAsync(t => t.Id == secGroupUpdateDto.Id);

            if (entity == null)
            {
                return new ParentResponseModel()
                {
                    ErrorCode = ErrorCatalog.ObjectNotFound,
                    IsDone = false,
                    ReturnMessage = "Object not found"
                };
            }
            entity.GroupName = secGroupUpdateDto.GroupName ?? entity.GroupName;

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
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> Update ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }

    public async Task<ParentResponseModel> GetGroupsWithEmployeesAndJobsBasedOnModule(int ModuleId)
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecGroups
                .Where(e => e.SecModuleGroups.Any(e => e.ModuleCode == ModuleId)).AsNoTracking()
                .Select(e => new SecGroupEmployeejobDto
                {
                    Id = e.Id,
                    GroupName = e.GroupName,
                    Employees = e.SecGroupEmployees.Select(e => e.EmpCode).ToList(),
                    Jobs = e.SecGroupJobs.Select(e => e.JobCode).ToList()
                }).ToListAsync();

            return new ListOfObjectsResponseModel<SecGroupEmployeejobDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Objects Loaded Successufly",
                Objects = listOfObjects
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> GetGroupsWithEmployeesAndJobsBasedOnModule ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }
    public async Task<ParentResponseModel> AssignJobsEmployeesToGroup(AssignJobsEmployeesToGroup assignJobsEmployeesToGroup)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            if (assignJobsEmployeesToGroup.ForEmployee)
            {
                var newEmployees = assignJobsEmployeesToGroup.EmployeeIds.Select(e => new SecGroupEmployee
                {
                    GroupCode = assignJobsEmployeesToGroup.GroupId,
                    EmpCode = e,
                    InsertDate = DateTime.Now,
                    InsertUserCode = userCode
                });
                await RepositoryContext.SecGroupEmployees.AddRangeAsync(newEmployees);
            }
            else
            {
                var newJobs = assignJobsEmployeesToGroup.JobsIds.Select(e => new SecGroupJob
                {
                    GroupCode = assignJobsEmployeesToGroup.GroupId,
                    JobCode = e,
                    InsertDate = DateTime.Now,
                    InsertUserCode = userCode
                });
                await RepositoryContext.SecGroupJobs.AddRangeAsync(newJobs);
            }
            await RepositoryContext.SaveChangesAsync();

            return new SingleObjectResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Assign Delete Page From Group Seccessfully"
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> AssignJobsEmployeesToGroup ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }
    public async Task<ParentResponseModel> DeleteEmployeeOrJobFromGroup(DeleteJobEmployeeFromGroup deleteJobEmployeeFromGroup)
    {
        try
        {
            string userCode = _httpContextAccessor?.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            ;

            if (deleteJobEmployeeFromGroup.ForEmployee)
            {
                var entity = await RepositoryContext.SecGroupEmployees.FirstOrDefaultAsync(e => e.GroupCode == deleteJobEmployeeFromGroup.GroupId &&
                                e.EmpCode == deleteJobEmployeeFromGroup.DeletedId);
                if (entity == null)
                    throw new Exception("Object Is Null");
                entity.DeleteUserCode = userCode;
                entity.DeleteDate = DateTime.Now;
                entity.IsDeleted = true;
            }
            else
            {
                var entity = await RepositoryContext.SecGroupJobs.FirstOrDefaultAsync(e => e.GroupCode == deleteJobEmployeeFromGroup.GroupId &&
                                e.JobCode == deleteJobEmployeeFromGroup.DeletedId);
                if (entity == null)
                    throw new Exception("Object Is Null");
                entity.DeleteUserCode = userCode;
                entity.DeleteDate = DateTime.Now;
                entity.IsDeleted = true;
            }
            await RepositoryContext.SaveChangesAsync();
            return new SingleObjectResponseModel<SecControlListDto>()
            {
                ErrorCode = ErrorCatalog.noError,
                IsDone = true,
                ReturnMessage = "Delete Employee Or Job From Group Successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.logErrorWithException(ex, $"{typeof(SecModule).Name} ===> DeleteEmployeeOrJobFromGroup ");
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
            var entity = await RepositoryContext.SecGroups
                .Include(e => e.SecModuleGroups).Include(e => e.SecGroupPages).Include(e => e.SecGroupControls)
                .Include(e=>e.SecGroupJobs).Include(e=>e.SecGroupEmployees).FirstOrDefaultAsync(t => t.Id == id);
            string userCode = _httpContextAccessor.HttpContext?.User.FindFirst("EMP_SERIAL")?.Value;
            if (entity != null)
            {
                if (entity.SecGroupPages.Count > 0 || entity.SecGroupEmployees.Count > 0 || entity.SecGroupControls.Count > 0 || entity.SecGroupJobs.Count > 0)
                {
                    return new ParentResponseModel()
                    {
                        ErrorCode = ErrorCatalog.VioleteConstrains,
                        IsDone = false,
                        ReturnMessage = "object has child object ModuleGroups||GroupPages||GroupEmployees||GroupControls||GroupJobs"
                    };
                }
                if(entity.SecModuleGroups.Count > 0)
                    RepositoryContext.SecModuleGroups.RemoveRange(entity.SecModuleGroups);
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
            _logger.logErrorWithException(ex, $"{typeof(SecGroup).Name} ===> Delete ");
            return new ParentResponseModel()
            {
                ErrorCode = ErrorCatalog.DataBaseFauiler,
                IsDone = false,
                ReturnMessage = ex.Message,
            };
        }
    }
    public async Task<ParentResponseModel> GetModulesForLookups()
    {
        try
        {
            var listOfObjects = await RepositoryContext.SecModules.AsNoTracking().Where(e=> (bool)e.IsTaken).
                Select(e => new SecModuleDto
                {
                    Id = e.Id,
                    ModuleName = e.ModuleName,
                    Icon = e.Icon,
                    Color=e.Color
                }).ToListAsync();


            return new ListOfObjectsResponseModel<SecModuleDto>()
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


}

