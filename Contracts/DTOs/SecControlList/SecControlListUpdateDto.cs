using Contracts.BaseDtos;

namespace Contracts.DTOs.SecControlList;

public class SecControlListUpdateDto:BaseUpdateDto
{
    public string ControlCode { get; set; } = null!;
    public string ControlDescription { get; set; } = null!;
}
