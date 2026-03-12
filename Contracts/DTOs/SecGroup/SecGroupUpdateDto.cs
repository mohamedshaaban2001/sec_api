using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroup;

public class SecGroupUpdateDto:BaseUpdateDto
{
    public string? GroupName { get; set; }
}
