
using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecGroupPage;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecGroupRepository : IRepositoryBase<SecGroup, SecGroupDto, SecGroupCreateDto, SecGroupUpdateDto>
{
    Task<ParentResponseModel> AssignJobsEmployeesToGroup(AssignJobsEmployeesToGroup assignJobsEmployeesToGroup);
    Task<ParentResponseModel> AvailableModulesWithGroup(int groupId);
    Task<ParentResponseModel> DeleteEmployeeOrJobFromGroup(DeleteJobEmployeeFromGroup deleteJobEmployeeFromGroup);
    Task<ParentResponseModel> GetGroupsWithEmployeesAndJobsBasedOnModule(int ModuleId);
    Task<ParentResponseModel> GetModulesForLookups();
}
