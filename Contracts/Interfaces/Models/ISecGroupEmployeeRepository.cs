
using Contracts.DTOs.SecGroupEmployee;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecGroupEmployeeRepository : IRepositoryBase<SecGroupEmployee, SecGroupEmployeeDto, SecGroupEmployeeCreateDto, SecGroupEmployeeUpdateDto>
{
    Task<ParentResponseModel> GetGroupsWithEmployeesAndJobsFromGrpc();
}
