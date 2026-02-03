using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Entities.Models.Databases.OracleDb
{
    public partial class OracleContext : RepositoryContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OracleContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("OracleConnection");
                optionsBuilder.UseOracle(connectionString, oracleOptions =>
                {
                    oracleOptions.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19);
                }).UseUpperSnakeCaseNamingConvention();
            }
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<bool>().HaveConversion<BoolToNumberConverter>().HaveColumnType("NUMBER(1,0)");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var namespaces = new[] { "Entities.Models.Configurations", "Entities.Models.Databases.OracleDb.Configurations" };
            ApplyConfiguration(modelBuilder, namespaces);
            //modelBuilder.Entity<MEMOS_FROM>()
            //   .Property(e => e.MEMO_ID)
            //   .HasDefaultValueSql("seq_example.NEXTVAL");
        }

    }
}
