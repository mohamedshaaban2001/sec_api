namespace Contracts.DTOs.SecGroupPage;

public class DeleteJobEmployeeFromGroup
{
    public int GroupId { get; set; }
    public int DeletedId { get; set; }
    public bool ForEmployee { get; set; }
}