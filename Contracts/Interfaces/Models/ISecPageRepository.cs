
using Contracts.DTOs.SecPage;
using Contracts.interfaces.Repository;
using Contracts.Responses;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecPageRepository : IRepositoryBase<SecPage, SecPageDto, SecPageCreateDto, SecPageUpdateDto>
{
    Task<ParentResponseModel> CreateControlForPage(AddControlToPage assignControlToPage);
    Task<ParentResponseModel> GetLookupsForCreatePage();
    Task<ParentResponseModel> GetPagesForLookup();
}
