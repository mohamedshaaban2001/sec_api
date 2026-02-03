

using System.ComponentModel.DataAnnotations;

namespace Contracts.BaseDtos;

public class BaseUpdateDto
{
    [Required]
    public int Id { get; set; }
}
