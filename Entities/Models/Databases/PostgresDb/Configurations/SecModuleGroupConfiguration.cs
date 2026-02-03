using Entities.Models.BaseConfiguration;
using Entities.Models.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models.Databases.PostgresDb.Configurations;

public class SecModuleGroupConfiguration : BaseConfiguration<SecModuleGroup>
{
    public override void Configure(EntityTypeBuilder<SecModuleGroup> entity)
    {


        base.Configure(entity);
        entity.HasIndex(e => new { e.GroupCode, e.ModuleCode })
              .IsUnique().HasFilter("is_deleted = FALSE"); ;
        entity.HasOne(d => d.SecModule)
            .WithMany(p => p.SecModuleGroups)
            .HasForeignKey(d => d.ModuleCode).OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(d => d.SecGroup)
            .WithMany(p => p.SecModuleGroups)
            .HasForeignKey(d => d.GroupCode).OnDelete(DeleteBehavior.Restrict);


    }

}
