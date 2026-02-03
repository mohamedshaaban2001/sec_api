
using Contracts.DTOs.SecGroupJob;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecGroupJobRepository : IRepositoryBase<SecGroupJob,SecGroupJobDto,SecGroupJobCreateDto,SecGroupJobUpdateDto>
{
}
