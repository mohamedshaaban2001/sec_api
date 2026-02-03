
using Contracts.DTOs.SecGroupControl;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecGroupControlRepository : IRepositoryBase<SecGroupControl,SecGroupControlDto,SecGroupControlCreateDto,SecGroupControlUpdateDto>
{
}
