using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroupPage;

public class SecGroupPageDto:BaseDto
{
    public int GroupId { get; set; }
    public int PageId { get; set; }
    public string PageName { get; set; }
    public string PageIcon { get; set; }
    public List<ControlsForSpecificPageAndGroup> Controls { get; set; } 
}

public class ControlsForSpecificPageAndGroup 
{
    public int ControlId { get; set; }
    public string ControlName { get; set; }
    public bool IsAssigned { get; set; }
}
