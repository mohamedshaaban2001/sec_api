namespace Contracts.DTOs.SecGroup;

/// <summary>
/// Job assigned to a group. <see cref="Id"/> is <c>JobCode</c>: use in
/// <c>AssignJobsEmployeesToGroup.JobsIds</c> and <c>DeleteEmployeeOrJobFromGroup.DeletedId</c> when <c>ForEmployee</c> is false.
/// </summary>
public class GroupMemberJobDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
