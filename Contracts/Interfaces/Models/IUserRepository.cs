
using Contracts.DTOs.User;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface IUserRepository : IRepositoryBase<User,UserDto,UserCreateDto,UserUpdateDto>
{
    /// <summary>Active users with person name, plus persons not linked to any user via <c>emp_serial</c>.</summary>
    Task<ParentResponseModel> GetUsersManagementPageData();

    Task<ParentResponseModel> GetUsers();
    Task<ParentResponseModel> ResetPassword(int userId);
    Task<ParentResponseModel> ChangePassword(string userCode, string oldPassword, string newPassword);
}
