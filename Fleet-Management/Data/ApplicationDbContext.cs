using System;
using Microsoft.EntityFrameworkCore;
using Fleet_Management.Models; 

namespace Fleet_Management.Data
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vehicle> Vehicles { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<VehicleInformation> VehicleInformations { get; set; }
        public virtual DbSet<RouteHistory> RouteHistories { get; set; }
        public virtual DbSet<Geofence> Geofences { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserRole> UserRoles { get; set; }

       
        public virtual DbSet<CircleGeofence> CircleGeofence { get; set; }
        public virtual DbSet<RectangleGeofence> RectangleGeofence { get; set; }
        public virtual DbSet<PolygonGeofence> PolygonGeofence { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<VehicleInformation>()
                .HasOne(vi => vi.Vehicle)
                .WithOne(v => v.VehicleInformation)
                .HasForeignKey<VehicleInformation>(vi => vi.VehicleID);

            modelBuilder.Entity<VehicleInformation>()
                .HasOne(vi => vi.Driver)
                .WithMany(d => d.VehicleInformations)
                .HasForeignKey(vi => vi.DriverID);
    
            modelBuilder.Entity<Vehicle>()
                   .HasIndex(v => v.VehicleNumber)
                   .IsUnique();

            modelBuilder.Entity<Driver>()
                .HasKey(d => d.DriverID);

            modelBuilder.Entity<RouteHistory>()
                .HasKey(rh => rh.RouteHistoryID);

            modelBuilder.Entity<RouteHistory>()
                .HasOne(rh => rh.Vehicle)
                .WithMany(v => v.RouteHistories)
                .HasForeignKey(rh => rh.VehicleID);



            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("User_pkey");

                entity.ToTable("User");

                entity.Property(e => e.UserId).HasColumnName("UserId");
                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");
                entity.Property(e => e.PasswordHash)
                    .HasMaxLength(255)
                    .HasColumnName("passwordhash");
                entity.Property(e => e.RoleId).HasColumnName("RoleId");  
                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .HasColumnName("username");

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId) 
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserRole");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.RoleId).HasName("UserRole_pkey");

                entity.ToTable("UserRole");

                entity.Property(e => e.RoleId).HasColumnName("RoleId"); 
                entity.Property(e => e.RoleName)
                    .HasMaxLength(255)
                    .HasColumnName("rolename");
            });

            modelBuilder.Entity<Geofence>()
        .HasOne(g => g.CircleGeofence)
        .WithOne(c => c.Geofence)
        .HasForeignKey<CircleGeofence>(c => c.GeofenceID);

            modelBuilder.Entity<Geofence>()
                .HasOne(g => g.RectangleGeofence)
                .WithOne(r => r.Geofence)
                .HasForeignKey<RectangleGeofence>(r => r.GeofenceID);

            modelBuilder.Entity<Geofence>()
                .HasMany(g => g.PolygonGeofences)
                .WithOne(p => p.Geofence)
                .HasForeignKey(p => p.GeofenceID);

        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

