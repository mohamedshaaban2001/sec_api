
using Contracts.DTOs.SecGroup;
using Contracts.DTOs.SecGroupPage;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecGroupRepository : IRepositoryBase<SecGroup, SecGroupDto, SecGroupCreateDto, SecGroupUpdateDto>
{
    Task<ParentResponseModel> AssignJobsEmployeesToGroup(AssignJobsEmployeesToGroup assignJobsEmployeesToGroup);
    Task<ParentResponseModel> DeleteEmployeeOrJobFromGroup(DeleteJobEmployeeFromGroup deleteJobEmployeeFromGroup);
    Task<ParentResponseModel> GetEmployeesForGroup(int groupId);
    Task<ParentResponseModel> GetJobsForGroup(int groupId);
}
