using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroup;

public class SecGroupCreateDto:BaseCreateDto
{
    public string GroupName { get; set; }
    public List<int> ModuleIds { get; set; }

}
