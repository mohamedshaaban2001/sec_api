
using Contracts.DTOs.SecModule;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecModuleRepository : IRepositoryBase<SecModule, SecModuleDto, SecModuleCreateDto, SecModuleUpdateDto>
{
    Task<ParentResponseModel> AssignTakenToServiceInModule(AssignTakenServiceForModule assignTakenServiceForModule);
    Task<ParentResponseModel> CreateServicesForModules(AssignServiceForModule assignServiceForModule);
    Task<ParentResponseModel> GetModulesWithTakenServices();
    Task<ParentResponseModel> GetServicesBasedOnModule(int ModuleId);
}
