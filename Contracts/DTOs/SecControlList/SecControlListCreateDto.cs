using Contracts.BaseDtos;

namespace Contracts.DTOs.SecControlList;

public class SecControlListCreateDto : BaseCreateDto
{
    /// <summary>Optional; when null the repository picks a fallback page (legacy behaviour).</summary>
    public int? PageId { get; set; }

    public string ControlCode { get; set; } = null!;
    public string ControlDescription { get; set; } = null!;
}
