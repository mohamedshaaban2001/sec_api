namespace Contracts.DTOs.SecGroupPage;

public class AssignControlsToPageInGroup
{
    public int PageId { get; set; }
    public int GroupId { get; set; }
    public List<int>? ControlIds { get; set; }
}
