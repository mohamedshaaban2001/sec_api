using System.ComponentModel.DataAnnotations;
using Contracts.BaseDtos;

namespace Contracts.DTOs.Privilege;

public class PrivilegeCreateDto : BaseCreateDto
{
    [Required]
    public int PageId { get; set; }

    [Required]
    public string Code { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;
}
