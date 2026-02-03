using Entities.Models.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Models.Databases.PostgresDb.Configurations
{
    public class PersonsConfiguration : IEntityTypeConfiguration<Persons>
    {
        public void Configure(EntityTypeBuilder<Persons> entity)
        {
            entity.HasNoKey();
            entity.ToView("persons", "sec");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NationalId).HasColumnName("national_id").HasMaxLength(50);
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasColumnName("middle_name").HasMaxLength(100);
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(100);
            entity.Property(e => e.FullName).HasColumnName("full_name").HasMaxLength(300);
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
            entity.Property(e => e.Mobile).HasColumnName("mobile").HasMaxLength(50);
            entity.Property(e => e.Address).HasColumnName("address").HasMaxLength(500);
            entity.Property(e => e.MilitaryRankId).HasColumnName("military_rank_id");
        }
    }
}
