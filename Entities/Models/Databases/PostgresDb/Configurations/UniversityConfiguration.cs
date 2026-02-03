using Entities.Models.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Models.Databases.PostgresDb.Configurations
{
    public class UniversityConfiguration : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> entity)
        {
            entity.HasNoKey();
            entity.ToView("universities", "sec");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameAr).HasColumnName("name_ar").HasMaxLength(255);
            entity.Property(e => e.NameEn).HasColumnName("name_en").HasMaxLength(255);
            entity.Property(e => e.ShortName).HasColumnName("short_name").HasMaxLength(100);
        }
    }
}
