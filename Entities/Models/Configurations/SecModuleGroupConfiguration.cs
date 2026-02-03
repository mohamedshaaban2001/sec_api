using Entities.Models.BaseConfiguration;
using Entities.Models.Tables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Entities.Models.Configurations;

public  class SecModuleGroupConfiguration : IEntityTypeConfiguration<SecModuleGroup>
{
    public  void Configure(EntityTypeBuilder<SecModuleGroup> entity)
    {

    }

}
