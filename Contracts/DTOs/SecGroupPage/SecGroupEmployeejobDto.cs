using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroupPage;

public class SecGroupEmployeejobDto : BaseDto
{
    public string GroupName { get; set; }
    public List<int>? Employees { get; set; }
    public List<int>? Jobs { get; set; }
}