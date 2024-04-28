using System;
using Microsoft.EntityFrameworkCore;
using Fleet_Management.Models; // This namespace should contain your model classes

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
        // Define other DbSet properties for your other entities like CircleGeofence, RectangleGeofence, and PolygonGeofence

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Existing configurations
            modelBuilder.Entity<VehicleInformation>()
                .HasOne(vi => vi.Vehicle)
                .WithOne(v => v.VehicleInformation)
                .HasForeignKey<VehicleInformation>(vi => vi.VehicleID);

            modelBuilder.Entity<VehicleInformation>()
                .HasOne(vi => vi.Driver)
                .WithMany(d => d.VehicleInformations)
                .HasForeignKey(vi => vi.DriverID);

            modelBuilder.Entity<Vehicle>()
                .HasKey(v => v.VehicleID);

            modelBuilder.Entity<Driver>()
                .HasKey(d => d.DriverID);

            modelBuilder.Entity<RouteHistory>()
                .HasKey(rh => rh.RouteHistoryID);

            modelBuilder.Entity<RouteHistory>()
                .HasOne(rh => rh.Vehicle)
                .WithMany(v => v.RouteHistories)
                .HasForeignKey(rh => rh.VehicleID);

            // Configuration for Geofence
            modelBuilder.Entity<Geofence>()
                .HasKey(g => g.GeofenceID);

            modelBuilder.Entity<CircleGeofence>()
                .HasKey(cg => cg.ID);

            modelBuilder.Entity<CircleGeofence>()
                .HasOne(cg => cg.Geofence)
                .WithOne(g => g.CircleGeofence)
                .HasForeignKey<CircleGeofence>(cg => cg.ID);

            modelBuilder.Entity<RectangleGeofence>()
                .HasKey(rg => rg.ID);

            modelBuilder.Entity<RectangleGeofence>()
                .HasOne(rg => rg.Geofence)
                .WithOne(g => g.RectangleGeofence)
                .HasForeignKey<RectangleGeofence>(rg => rg.ID);

            modelBuilder.Entity<PolygonGeofence>()
                .HasKey(pg => pg.ID);

            modelBuilder.Entity<PolygonGeofence>()
                .HasOne(pg => pg.Geofence)
                .WithMany(g => g.PolygonGeofences)
                .HasForeignKey(pg => pg.GeofenceID);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

