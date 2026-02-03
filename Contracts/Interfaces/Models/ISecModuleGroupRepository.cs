
using Contracts.DTOs.SecModuleGroup;
using Contracts.interfaces.Repository;
using Entities.Models.Tables;

namespace Contracts.interfaces.Models;

public interface ISecModuleGroupRepository : IRepositoryBase<SecModuleGroup,SecModuleGroupDto,SecModuleGroupCreateDto,SecModuleGroupUpdateDto>
{
}
