
using Contracts.DTOs.SecControlList;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecControlListRepository : IRepositoryBase<SecControlList, SecControlListDto, SecControlListCreateDto, SecControlListUpdateDto>
{
    Task<ParentResponseModel> FindAllForPage(int pageId);
}
