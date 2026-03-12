using Contracts.DTOs.User;
using System.Threading.Tasks;

namespace Service_API.Services
{
    public interface IKeycloakService
    {
        Task<bool> CreateUserAsync(UserCreateDto user, int personId);
        Task<bool> UpdateUserAsync(UserUpdateDto user, int personId);
        Task<bool> DeleteUserAsync(string username);
        Task<bool> CreateGroupAsync(string groupName);
        Task<bool> UpdateGroupAsync(string currentGroupName, string newGroupName);
        Task<bool> DeleteGroupAsync(string groupName);
        Task<List<string>> GetUserGroupsAsync(string userId);
    }
}
