using Contracts.BaseDtos;

namespace Contracts.DTOs.SecControlList;

public class SecControlListCreateDto:BaseCreateDto
{
    public string ControlCode { get; set; } = null!;
    public string ControlDescription { get; set; } = null!;
}
