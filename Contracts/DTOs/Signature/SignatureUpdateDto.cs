using Contracts.BaseDtos;

namespace Contracts.DTOs.Signature;

public class SignatureUpdateDto:BaseUpdateDto
{
    public string SignatureColor { get; set; }
    public int EmployeeId { get; set; }
}
