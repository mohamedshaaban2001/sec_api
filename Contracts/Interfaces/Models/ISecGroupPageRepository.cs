
using Contracts.DTOs.SecGroupPage;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecGroupPageRepository : IRepositoryBase<SecGroupPage, SecGroupPageDto, SecGroupPageCreateDto, SecGroupPageUpdateDto>
{
    Task<ParentResponseModel> AssignControlsToPageInGroup(AssignControlsToPageInGroup assignControlsToPageInGroup);
    Task<ParentResponseModel> AssignDeletePageFromGroup(AssignDeletePageFromGroup assignDeletePageFromGroup);
    Task<ParentResponseModel> FindPagesBasedOnGroupId(int groupId);
}
