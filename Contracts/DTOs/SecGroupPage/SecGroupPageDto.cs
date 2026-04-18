using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroupPage;

public class SecGroupPageDto : BaseDto
{
    public int GroupId { get; set; }
    public int PageId { get; set; }
    public string PageName { get; set; } = null!;
    public string? PageIcon { get; set; }
    public List<GroupPagePrivilegeDto> Privileges { get; set; } = null!;
}
