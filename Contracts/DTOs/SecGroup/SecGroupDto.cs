using Contracts.BaseDtos;

namespace Contracts.DTOs.SecGroup;

public class SecGroupDto:BaseDto
{
    public string? GroupName { get; set; }
    public List<Dictionary<string, string>>? Headers { get; set; }
    public List<Dictionary<string, object>>? Rows { get; set; }       

}

public class ModulesInGroup
{
    public int ModuleId { get; set; }
    public string ModuleName { get; set; }
    public bool IsAssigned { get; set; }

}
