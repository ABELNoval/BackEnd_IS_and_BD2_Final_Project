using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.ValueObjects;
using Domain.Enumerations;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para entidades con identidad propia
        public DbSet<User> Users { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Technical> Technicals { get; set; }
        public DbSet<Responsible> Responsibles { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<EquipmentDecommission> EquipmentDecommissions { get; set; }
        
        // Smart Enums NO necesitan DbSet - son valores en código, no tablas

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// <summary>
            /// Configures TPT (Table Per Type) inheritance for User hierarchy.
            /// </summary>
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Director>().ToTable("Directors");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Technical>().ToTable("Technicals");
            modelBuilder.Entity<Responsible>().ToTable("Responsibles");

            modelBuilder.Entity<Employee>().HasBaseType<User>();
            modelBuilder.Entity<Responsible>().HasBaseType<Employee>();
            modelBuilder.Entity<Technical>().HasBaseType<User>();
            modelBuilder.Entity<Director>().HasBaseType<User>();

            /// <summary>
            /// Configures Assessment entity with PerformanceScore value object converter.
            /// </summary>
            modelBuilder.Entity<Assessment>(entity =>
            {
                entity.ToTable("Assessments");
                
                entity.Property(a => a.Score)
                    .HasConversion(
                        v => v.Value,
                        v => PerformanceScore.Create(v)
                    )
                    .HasColumnName("Score")
                    .HasPrecision(5, 2)
                    .IsRequired();

                entity.Property(a => a.Comment)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(a => a.AssessmentDate)
                    .IsRequired();

                entity.HasIndex(a => a.AssessmentDate);
            });

            /// <summary>
            /// Configures Equipment entity with smart enum converters for State and LocationType.
            /// </summary>
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("Equipments");

                entity.Property(e => e.StateId)
                    .HasColumnName("StateId")
                    .IsRequired();

                entity.Property(e => e.LocationTypeId)
                    .HasColumnName("LocationTypeId")
                    .IsRequired();

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsRequired();

                entity.Property(e => e.AcquisitionDate)
                    .IsRequired();

                entity.Property(e => e.EquipmentTypeId)
                    .IsRequired();

                entity.Property(e => e.DepartmentId)
                    .IsRequired(false);

                entity.HasMany(e => e.Transfers)
                    .WithOne()
                    .HasForeignKey("EquipmentId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Maintenances)
                    .WithOne()
                    .HasForeignKey("EquipmentId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Decommissions)
                    .WithOne()
                    .HasForeignKey("EquipmentId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            /// <summary>
            /// Configures User base entity with property constraints and unique email index.
            /// </summary>
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                // Map Email and PasswordHash value objects to simple string columns
                entity.Property(u => u.Email)
                    .HasConversion(
                        v => v.Value,
                        v => Domain.ValueObjects.Email.Create(v)
                    )
                    .HasMaxLength(150)
                    .IsRequired();

                entity.Property(u => u.PasswordHash)
                    .HasConversion(
                        v => v.Value,
                        v => Domain.ValueObjects.PasswordHash.Create(v)
                    )
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(u => u.RoleId)
                    .HasColumnName("RoleId")
                    .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();
            });

            /// <summary>
            /// Configures Technical entity with specialty and experience constraints.
            /// </summary>
            modelBuilder.Entity<Technical>(entity =>
            {
                entity.Property(t => t.Specialty)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(t => t.Experience)
                    .IsRequired();

                entity.HasMany(t => t.Assessments)
                    .WithOne()
                    .HasForeignKey("TechnicalId")
                    .OnDelete(DeleteBehavior.Cascade);
            });

            /// <summary>
            /// Configures Maintenance entity with cost precision and date indexing.
            /// </summary>
            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.ToTable("Maintenances");

                entity.Property(m => m.MaintenanceTypeId)
                    .HasColumnName("MaintenanceTypeId")
                    .IsRequired();

                entity.Property(m => m.Cost)
                    .HasPrecision(10, 2)
                    .IsRequired();

                entity.Property(m => m.MaintenanceDate)
                    .IsRequired();

                entity.HasIndex(m => m.MaintenanceDate);
            });

            /// <summary>
            /// Configures Transfer entity with date tracking and indexing.
            /// </summary>
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.ToTable("Transfers");

                entity.Property(t => t.TransferDate)
                    .IsRequired();

                entity.Property(t => t.CreatedAt)
                    .IsRequired();

                entity.HasIndex(t => t.TransferDate);
            });

            /// <summary>
            /// Configures EquipmentDecommission entity with reason constraints and date indexing.
            /// </summary>
            modelBuilder.Entity<EquipmentDecommission>(entity =>
            {
                entity.ToTable("EquipmentDecommissions");

                entity.Property(d => d.DestinyTypeId)
                    .HasColumnName("DestinyTypeId")
                    .IsRequired();

                entity.Property(d => d.Reason)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(d => d.DecommissionDate)
                    .IsRequired();

                entity.HasIndex(d => d.DecommissionDate);
            });

            /// <summary>
            /// Configures Department entity with name constraints and foreign key references.
            /// </summary>
            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Departments");

                entity.Property(d => d.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(d => d.SectionId)
                    .IsRequired();
            });

            /// <summary>
            /// Configures Section entity with name constraints.
            /// </summary>
            modelBuilder.Entity<Section>(entity =>
            {
                entity.ToTable("Sections");

                entity.Property(s => s.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                // use 's' variable name for clarity
                entity.Property(s => s.ResponsibleId)
                    .IsRequired();
            });

            /// <summary>
            /// Configures EquipmentType entity with name constraints and unique index.
            /// </summary>
            modelBuilder.Entity<EquipmentType>(entity =>
            {
                entity.ToTable("EquipmentTypes");

                entity.Property(et => et.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.HasIndex(et => et.Name)
                    .IsUnique();
            });
        }
    }
}