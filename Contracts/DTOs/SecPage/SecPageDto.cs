using Contracts.BaseDtos;
using Contracts.DTOs.Privilege;

namespace Contracts.DTOs.SecPage;

public class SecPageDto : BaseDto
{
    public string? PageName { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public int? PageOrder { get; set; }
    public string PageUrl { get; set; } = null!;
    public string? Icon { get; set; }
    public int? ServiceCode { get; set; }
    public string? ServiceName { get; set; }

    /// <summary>Privileges (actions) defined for this page.</summary>
    public List<PrivilegeDto>? Privileges { get; set; }
}
