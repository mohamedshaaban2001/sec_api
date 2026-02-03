using Contracts.BaseDtos;

namespace Contracts.DTOs.SecModule;

public class SecModuleUpdateDto:BaseUpdateDto
{
    public string? ModuleName { get; set; }
    public string? Icon { get; set; }
    public string? Color { get; set; }

}
