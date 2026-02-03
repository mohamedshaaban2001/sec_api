namespace Contracts.DTOs.SecGroupPage;

public class AssignJobsEmployeesToGroup
{
    public int GroupId { get; set; }
    public List<int>? EmployeeIds { get; set; }
    public List<int>? JobsIds { get; set; }
    public bool ForEmployee { get; set; }
}
