using Contracts.BaseDtos;

namespace Contracts.DTOs.SecModule;

public class SecModuleDto:BaseDto
{
    public string ModuleName { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

}
