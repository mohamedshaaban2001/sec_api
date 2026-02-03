using Contracts.BaseDtos;

namespace Contracts.DTOs.Signature;

public class SignatureDto:BaseDto
{
    public string SignatureColor { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string JobName { get; set; }
    public string DepartmentName { get; set; }
}
