using Contracts.BaseDtos;

namespace Contracts.DTOs.Signature;

public class SignatureCreateDto:BaseCreateDto
{
    public string SignatureColor { get; set; }
    public int EmployeeId { get; set; }
}
