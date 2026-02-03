using Entities.Models.BaseTables;
using Entities.Models.Tables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Entities.Models.BaseConfiguration;

public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseTable
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {


        //builder.HasKey(e=>e.Id).HasName($"PK_{typeof(TEntity).Name}");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
       .UseIdentityColumn();
        builder.HasQueryFilter(e => !e.IsDeleted);

    }

}

