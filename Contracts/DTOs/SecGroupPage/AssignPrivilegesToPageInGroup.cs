namespace Contracts.DTOs.SecGroupPage;

public class AssignPrivilegesToPageInGroup
{
    public int PageId { get; set; }
    public int GroupId { get; set; }
    public List<int>? PrivilegeIds { get; set; }
}
