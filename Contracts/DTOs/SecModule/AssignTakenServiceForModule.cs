namespace Contracts.DTOs.SecModule;

public class AssignTakenServiceForModule 
{
    public int ModuleId { get; set; }
    public bool ModuleAssigned { get; set; }
    public List<int>? ServiceTakens { get; set; }
}

