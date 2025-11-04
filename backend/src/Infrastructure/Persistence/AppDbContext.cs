using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets para cada entidad
        public DbSet<User> Users { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Technical> Technicals { get; set; }
        public DbSet<Responsible> Responsibles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DestinyType> DestinyTypes { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<TechnicalDowntime> TechnicalDowntimes { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TechnicalDowntime>(b =>
            {
                b.HasKey(td => td.Id);

                b.HasOne(td => td.Technical)
                 .WithMany()
                 .HasForeignKey(td => td.TechnicalId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(td => td.Equipment)
                 .WithMany()
                 .HasForeignKey(td => td.EquipmentId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(td => td.DestinyType)
                 .WithMany()
                 .HasForeignKey(td => td.DestinyTypeId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(td => td.Department)
                 .WithMany()
                 .HasForeignKey(td => td.DepartmentId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // CONFIGURACIÓN TPT PARA LA JERARQUÍA COMPLETA
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Director>().ToTable("Directors");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Technical>().ToTable("Technicals");
            modelBuilder.Entity<Responsible>().ToTable("Responsibles");

            // Configuración explícita de la herencia User -> Employee -> Responsible
            modelBuilder.Entity<Employee>()
                .HasBaseType<User>();

            modelBuilder.Entity<Responsible>()
                .HasBaseType<Employee>();

            // Technical y Director heredan directamente de User
            modelBuilder.Entity<Technical>()
                .HasBaseType<User>();

            modelBuilder.Entity<Director>()
                .HasBaseType<User>();

            // Configuración de claves foráneas para las agregaciones

            // Assessment - Combinación de Director y Technical
            modelBuilder.Entity<Assessment>(entity =>
            {
                entity.HasOne(a => a.Director)
                    .WithMany()
                    .HasForeignKey("DirectorId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Technical)
                    .WithMany()
                    .HasForeignKey("TechnicalId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // TechnicalDowntime - Agregación compleja (4 relaciones)
            modelBuilder.Entity<TechnicalDowntime>(entity =>
            {
                entity.HasOne(td => td.Technical)
                    .WithMany()
                    .HasForeignKey("TechnicalId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(td => td.Equipment)
                    .WithMany()
                    .HasForeignKey("EquipmentId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(td => td.DestinyType)
                    .WithMany(dt => dt.TechnicalDowntimes)
                    .HasForeignKey("DestinyTypeId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(td => td.Department)
                    .WithMany()
                    .HasForeignKey("DepartmentId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Maintenance - Combinación de Equipment y Technical
            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasOne(m => m.Equipment)
                    .WithMany()
                    .HasForeignKey("EquipmentId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(m => m.Technical)
                    .WithMany()
                    .HasForeignKey("TechnicalId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Transfer - Combinación entre Departamentos y Equipment
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity.HasOne(t => t.Origin)
                    .WithMany()
                    .HasForeignKey("OriginId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Destiny)
                    .WithMany()
                    .HasForeignKey("DestinyId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Equipment)
                    .WithMany()
                    .HasForeignKey("EquipmentId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Relaciones de Department
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasOne(d => d.Section)
                    .WithMany(s => s.Departaments)
                    .HasForeignKey("SectionId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Responsible)
                    .WithMany()
                    .HasForeignKey("ResponsibleId")
                    .OnDelete(DeleteBehavior.Restrict);
            });
             
            // Relaciones de Equipment
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasOne(e => e.EquipmentType)
                    .WithMany(et => et.Equipments)
                    .HasForeignKey("EquipmentTypeId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Equipments)
                    .HasForeignKey("DepartmentId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Relaciones de Employee (Responsible hereda esta relación)
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey("DepartmentId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Relaciones de User (base de toda la jerarquía)
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de propiedades requeridas
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Gmail)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Technical>()
                .Property(t => t.Specialty)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Equipment>()
                .Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Equipment>()
                .Property(e => e.State)
                .IsRequired()
                .HasMaxLength(50);

            // Índices para mejorar el rendimiento
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Gmail)
                .IsUnique();

            modelBuilder.Entity<TechnicalDowntime>()
                .HasIndex(td => td.Date);

            modelBuilder.Entity<Maintenance>()
                .HasIndex(m => m.DateTime);

            modelBuilder.Entity<Transfer>()
                .HasIndex(t => t.DateTime);

            modelBuilder.Entity<Assessment>()
                .HasIndex(a => a.Date);

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.DepartmentId);
        }
    }
}