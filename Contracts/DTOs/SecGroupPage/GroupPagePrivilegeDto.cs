namespace Contracts.DTOs.SecGroupPage;

/// <summary>
/// Privilege row when editing a group's access to a page (assign / revoke in UI).
/// </summary>
public class GroupPagePrivilegeDto
{
    public int PrivilegeId { get; set; }
    public string PrivilegeName { get; set; } = null!;
    public bool IsAssigned { get; set; }
}
