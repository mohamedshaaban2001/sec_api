#nullable enable

namespace Entities.Models.Views
{
    public class University
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = null!;
        public string? NameEn { get; set; }
        public string? ShortName { get; set; }
    }
}
