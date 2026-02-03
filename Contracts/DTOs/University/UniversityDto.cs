namespace Contracts.DTOs.University
{
    public class UniversityDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public string? ShortName { get; set; }
    }

    public class UniversityCreateDto {}
    public class UniversityUpdateDto {}
}
