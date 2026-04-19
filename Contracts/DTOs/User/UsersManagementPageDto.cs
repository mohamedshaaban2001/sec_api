namespace Contracts.DTOs.User;

/// <summary>Person from <c>persons</c> view with no matching <c>users.emp_serial</c> (active users only).</summary>
public class PersonWithoutUserDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
}

/// <summary>Payload for the user-admin screen: existing users and people who can still get an account.</summary>
public class UsersManagementPageDto
{
    public List<UserNameDto> Users { get; set; } = new();
    public List<PersonWithoutUserDto> PersonsWithoutUser { get; set; } = new();
}
