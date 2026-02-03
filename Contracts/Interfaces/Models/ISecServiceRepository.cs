
using Contracts.DTOs.SecService;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecServiceRepository : IRepositoryBase<SecService,SecServiceDto,SecServiceCreateDto,SecServiceUpdateDto>
{
}
