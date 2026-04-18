namespace Contracts.DTOs.SecGroup;

/// <summary>
/// Employee assigned to a group. <see cref="Id"/> is <c>EmpCode</c> (person id): use in
/// <c>AssignJobsEmployeesToGroup.EmployeeIds</c> and <c>DeleteEmployeeOrJobFromGroup.DeletedId</c> when <c>ForEmployee</c> is true.
/// </summary>
public class GroupMemberEmployeeDto
{
    public int Id { get; set; }
    public string EmployeeName { get; set; } = null!;
}
