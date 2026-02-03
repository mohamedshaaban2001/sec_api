namespace Contracts.DTOs.Job
{
    public class JobDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class JobCreateDto {}
    public class JobUpdateDto {}
}
