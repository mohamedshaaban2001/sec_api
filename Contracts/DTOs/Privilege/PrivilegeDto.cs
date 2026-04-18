using Contracts.BaseDtos;

namespace Contracts.DTOs.Privilege;

/// <summary>
/// Application privilege (maps to SEC_CONTROLS_LIST / SecControlList).
/// Always belongs to exactly one page.
/// </summary>
public class PrivilegeDto : BaseDto
{
    public int PageId { get; set; }
    public string Code { get; set; } = null!;
    public string Description { get; set; } = null!;
}
