using Contracts.BaseDtos;

namespace Contracts.DTOs.SecService;

public class SecServiceDto:BaseDto
{
    public int? ModuleNo { get; set; }
    public string ServiceName { get; set; } = null!;

}
