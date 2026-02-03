using Entities.Models.BaseConfiguration;
using Entities.Models.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Models.Databases.PostgresDb.Configurations;

public class SignatureConfiguration : BaseConfiguration<Signature>
{
    public override void Configure(EntityTypeBuilder<Signature> entity)
    {
        base.Configure(entity);
        entity.Property(e => e.SignatureColor).HasMaxLength(50);

    }

}