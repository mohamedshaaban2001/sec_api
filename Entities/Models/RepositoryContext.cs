using Entities.Models.Audit;
using Entities.Models.Tables;
using Entities.Models.Views;
//using Entities.Models.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Threading;

namespace Entities.Models
{
    public class RepositoryContext : DbContext
    {


        public virtual DbSet<Signature> Signatures { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<SecModuleGroup> SecModuleGroups { get; set; } = null!;

        public virtual DbSet<SecService> SecServices { get; set; } = null!;

        public virtual DbSet<SecPage> SecPages { get; set; } = null!;

        public virtual DbSet<SecModule> SecModules { get; set; } = null!;

        public virtual DbSet<SecGroupPage> SecGroupPages { get; set; } = null!;

        public virtual DbSet<SecGroupJob> SecGroupJobs { get; set; } = null!;

        public virtual DbSet<SecGroupEmployee> SecGroupEmployees { get; set; } = null!;

        public virtual DbSet<SecGroupControl> SecGroupControls { get; set; } = null!;

        public virtual DbSet<SecGroup> SecGroups { get; set; } = null!;

        public virtual DbSet<SecControlList> SecControlLists { get; set; } = null!;
        public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;




        protected readonly IConfiguration Configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected RepositoryContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        protected void ApplyConfiguration(ModelBuilder modelBuilder, string[] namespaces)
        {
            var methodInfo = (typeof(ModelBuilder).GetMethods()).Single((e =>
                e.Name == "ApplyConfiguration" &&
                e.ContainsGenericParameters &&
                e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition() ==
                typeof(IEntityTypeConfiguration<>)));

            foreach (var configType in typeof(RepositoryContext)
                         .GetTypeInfo().Assembly
                         .GetTypes()
                         .Where(t => t.Namespace != null &&
                                     namespaces.Any(n => n == t.Namespace) &&
                                     t.GetInterfaces().Any(i => i.IsGenericType &&
                                                                i.GetGenericTypeDefinition() ==
                                                                typeof(IEntityTypeConfiguration<>)
                                     )
                         )
                    )
            {
                var type = configType.GetInterfaces().First();
                methodInfo.MakeGenericMethod(type.GenericTypeArguments[0]).Invoke(modelBuilder, new[]
                {
                Activator.CreateInstance(configType)
            });
            }
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //bool isRequiredColomns = await checkForRequiredProprties();
            //if (isRequiredColomns)
            //{
            var auditEntries = await OnBeforeSaveChanges();
            if (auditEntries.Count > 0) await AuditChanges(auditEntries);
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
            //}
            //return 0;
        }

        private async Task<List<AuditEntry>> OnBeforeSaveChanges()
        {
            //ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            foreach (var entry in ChangeTracker.Entries())
            {

                if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged || entry.State != EntityState.Modified) continue;


                var auditEntry = new AuditEntry(entry);
                auditEntry.TableName = entry.Entity.GetType().Name;

                foreach (var property in entry.OriginalValues.Properties)
                {

                    var original = entry.OriginalValues[property];
                    var current = entry.CurrentValues[property];

                    if (property.IsPrimaryKey())
                    {
                        auditEntry.TABLE_ID.Add($"{property.Name} => {original}");
                    }

                    if (!object.Equals(original, current))
                    {
                        auditEntry.Changes.Add($"{property.Name}: {original} => {current}");
                    }
                }

                if (auditEntry.Changes.Any())
                {
                    auditEntries.Add(auditEntry);
                }

            }

            return auditEntries;
        }
        private async Task AuditChanges(List<AuditEntry> auditEntries)
        {

            //string EMP_SERIAL = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "EMP_SERIAL").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
            foreach (var auditEntry in auditEntries)
            {
                await AuditLogs.AddAsync(new AuditLog
                {

                    TimeStamp = DateTime.Now,
                    //EMP_SERIAL = EMP_SERIAL,
                    TableName = auditEntry.TableName,
                    Changes = string.Join(", ", auditEntry.Changes),
                    TableId = string.Join(", ", auditEntry.TABLE_ID)


                });
            }
            //await SaveChangesAsync();
            //SaveChanges();
        }
        //private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        //{

        //    //string EMP_SERIAL = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == "EMP_SERIAL").Select(c => new { c.Type, c.Value }).ToList()[0].Value;
        //    foreach (var auditEntry in auditEntries)
        //    {
        //        AUDITLOGs.Add(new AUDITLOG
        //        {

        //            //TIMESTAMP = DateTime.Now,
        //            //EMP_SERIAL = EMP_SERIAL,
        //            TABLE_NAME = auditEntry.TableName,
        //            CHANGES = string.Join(", ", auditEntry.Changes),
        //            TABLE_ID = string.Join(", ", auditEntry.TABLE_ID)


        //        });
        //    }
        //    await SaveChangesAsync();
        //    //SaveChanges();
        //}
        public virtual DbSet<Persons> Persons { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<University> Universities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Persons>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Persons");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("Job");
            });

            modelBuilder.Entity<University>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("University");
            });
            
            ApplyConfiguration(modelBuilder, new[] { "Entities.Configurations", "Entities.BaseConfiguration" });
        }
    }
}
