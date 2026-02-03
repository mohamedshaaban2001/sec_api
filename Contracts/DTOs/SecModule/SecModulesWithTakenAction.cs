namespace Contracts.DTOs.SecModule;

public class SecModulesWithTakenAction 
{
    public int Id { get; set; }
    public string ModuleName { get; set; }
    public string ModuleIcon { get; set; }
    public string ModuleColor { get; set; }
    public bool IsModuleAssigned { get; set; }
    public List<ServiceTaken> serviceTakens { get; set; }


}
public class ServiceTaken
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public bool? IsServiceTaken { get; set; }
}
