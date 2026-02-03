using Contracts.BaseDtos;

namespace Contracts.DTOs.SecModule;

public class SecModuleSmallDto : BaseDto
{
    public string Name { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

}

