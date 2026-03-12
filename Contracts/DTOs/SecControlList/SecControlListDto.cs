using Contracts.BaseDtos;

namespace Contracts.DTOs.SecControlList;

public class SecControlListDto:BaseDto
{
    public string ControlCode { get; set; } = null!;
    public string ControlDescription { get; set; } = null!;
}
