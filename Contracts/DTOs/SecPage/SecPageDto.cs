using Contracts.BaseDtos;
using Contracts.DTOs.SecControlList;
using Contracts.DTOs.SecGroupPage;

namespace Contracts.DTOs.SecPage;

public class SecPageDto:BaseDto
{

    public string? PageName { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public int? PageOrder { get; set; }
    public string PageUrl { get; set; }

    public string? Icon { get; set; }
    public int ModuleCode { get; set; }
    public string ModuleName { get; set; }
    public int? ServiceCode { get; set; }
    public string? ServiceName { get; set; }
    //public List<SecGroupPageSmallDto>? Groups { get; set; }
    public List<SecControlListDto>? Controls { get; set; }
}
