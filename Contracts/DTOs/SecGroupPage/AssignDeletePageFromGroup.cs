namespace Contracts.DTOs.SecGroupPage;

public class AssignDeletePageFromGroup
{
    public List<int>? AssignedPageIds { get; set; }
    public int? DeletedPageId { get; set; }
    public int GroupId { get; set; }
    public bool AssignFlag { get; set; }
}
