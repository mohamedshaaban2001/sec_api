using Contracts.BaseDtos;

namespace Contracts.DTOs.SecPage;

public class SecPageUpdateDto:BaseUpdateDto
{
    public string PageName { get; set; }
    public int? ParentId { get; set; }
    public int PageOrder { get; set; }
    public string? PageUrl { get; set; }
    public string? Icon { get; set; }
    public int ModuleCode { get; set; }
    public int? ServiceCode { get; set; }
    //public List<int> GroupIds { get; set; }
}
