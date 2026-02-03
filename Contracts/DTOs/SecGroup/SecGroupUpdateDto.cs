using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroup;

public class SecGroupUpdateDto:BaseUpdateDto
{
    public string? GroupName { get; set; }
    public List<int>? ModuleIds { get; set; }
}
