using Entities.Models.Tables;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Entities.Models.Databases.PostgresDb
{
    public partial class PostgresContext : RepositoryContext
    {


        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PostgresContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("PostgresConnection");
                optionsBuilder.UseNpgsql(connectionString).UseSnakeCaseNamingConvention();
                //optionsBuilder.UseIdentityByDefaultColumns();
                optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.HasDefaultSchema("sec");

            var namespaces = new[] { "Entities.Models.Configurations", "Entities.Models.Databases.PostgresDb.Configurations" };
            ApplyConfiguration(modelBuilder, namespaces);

           // modelBuilder.HasSequence("AUDITLOG_SEQ")
           //.HasMin(0)
           //.HasMax(9999999999999999)
           //.IsCyclic();

            OnModelCreatingPartial(modelBuilder);



        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);        
    }
}
