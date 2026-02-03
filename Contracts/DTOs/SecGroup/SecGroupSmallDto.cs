using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroup;

public class SecGroupSmallDto :BaseDto
{
    public string Name { get; set; }
    public List<int>? ModuleIds { get; set; }

}
