using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QMSWebApplication.BackendServer.Data.Entities;

namespace QMSWebApplication.BackendServer.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
        ) : IdentityDbContext<User, Roles, string>(options)
    {
        public virtual DbSet<EventLogs> EventLogs { get; set; }
        public virtual DbSet<TVDisplays> TVDisplays { get; set; }
        public virtual DbSet<Samples> MeasDatas { get; set; }
        public virtual DbSet<SampleTypes> MeasureTypes { get; set; }
        public virtual DbSet<ProductionAreas> ProductionAreas { get; set; }
        public virtual DbSet<InspectionPlanTypes> InspPlanTypes { get; set; }
        public virtual DbSet<InspectionPlans> InspectionPlans { get; set; }
        public virtual DbSet<InspectionPlanSubs> InspectionPlanSubs { get; set; }
        public virtual DbSet<InspectionPlanData> InspectionPlanDatas { get; set; }
        public virtual DbSet<InspectionPlanTracking> InspectionPlanTracking { get; set; }
        public virtual DbSet<ProductionPlans> ProductionPlans { get; set; }
        public virtual DbSet<ProcessLines> ProcessLines { get; set; }
        public virtual DbSet<Processes> Processes { get; set; }
        public virtual DbSet<Characteristics> Characteristics { get; set; }
        public virtual DbSet<Products> Products { get; set; } 
        public virtual DbSet<JobDecisions> JobDecisions { get; set; }
        public virtual DbSet<Shifts> Shifts { get; set; } 
        public virtual DbSet<Jobs> JobDatas { get; set; }
        public virtual DbSet<EventRoles> AlarmEvents { get; set; }
        public virtual DbSet<EmailServer> EmailServers { get; set; }
        public virtual DbSet<WebSession> WebSessions { get; set; }
        public virtual DbSet<Functions> Functions { get; set; }
        public virtual DbSet<Commands> Commands { get; set; }
        public virtual DbSet<CommandInFuntions> CommandInFuntions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Roles>()
            .Property(r => r.IntRoleID)
            .ValueGeneratedNever(); // Prevent identity generation on this property


            modelBuilder.Entity<User>()
            .Property(r => r.IntUserID)
            .ValueGeneratedNever(); // Prevent identity generation on this property

            modelBuilder.HasSequence("KnowledgeBaseSepence");
        }
    }
}
