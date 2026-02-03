using Entities.Models.Views;
using System;

namespace Contracts.DTOs.Persons
{
    public class PersonsDto
    {
        public int Id { get; set; }
        public string NationalId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public Gender? Gender { get; set; }
        public DateOnly? Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public int? MilitaryRankId { get; set; }
    }

    public class PersonsCreateDto {}
    public class PersonsUpdateDto {}
}
