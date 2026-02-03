
using Contracts.DTOs.User;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface IUserRepository : IRepositoryBase<User,UserDto,UserCreateDto,UserUpdateDto>
{
    Task<ParentResponseModel> GetUsers();
    Task<ParentResponseModel> ResetPassword(int userId);
    Task<ParentResponseModel> ChangePassword(string userCode, string oldPassword, string newPassword);
}
