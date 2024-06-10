﻿// <auto-generated />
using Fleet_Management.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fleet_Management.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240503021753_Modifications included in the modified version")]
    partial class Modificationsincludedinthemodifiedversion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Fleet_Management.Models.CircleGeofence", b =>
                {
                    b.Property<long>("ID")
                        .HasColumnType("bigint");

                    b.Property<long>("GeofenceID")
                        .HasColumnType("bigint");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.Property<long>("Radius")
                        .HasColumnType("bigint");

                    b.HasKey("ID");

                    b.ToTable("CircleGeofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.Driver", b =>
                {
                    b.Property<long>("DriverID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("DriverID"));

                    b.Property<string>("DriverName")
                        .HasColumnType("text");

                    b.Property<long>("PhoneNumber")
                        .HasColumnType("bigint");

                    b.HasKey("DriverID");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("Fleet_Management.Models.Geofence", b =>
                {
                    b.Property<long>("GeofenceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("GeofenceID"));

                    b.Property<long>("AddedDate")
                        .HasColumnType("bigint");

                    b.Property<string>("FillColor")
                        .HasColumnType("text");

                    b.Property<double>("FillOpacity")
                        .HasColumnType("double precision");

                    b.Property<string>("GeofenceType")
                        .HasColumnType("text");

                    b.Property<string>("StrokeColor")
                        .HasColumnType("text");

                    b.Property<double>("StrokeOpacity")
                        .HasColumnType("double precision");

                    b.Property<float>("StrokeWeight")
                        .HasColumnType("real");

                    b.HasKey("GeofenceID");

                    b.ToTable("Geofences");
                });

            modelBuilder.Entity("Fleet_Management.Models.PolygonGeofence", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("ID"));

                    b.Property<long>("GeofenceID")
                        .HasColumnType("bigint");

                    b.Property<float>("Latitude")
                        .HasColumnType("real");

                    b.Property<float>("Longitude")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.HasIndex("GeofenceID");

                    b.ToTable("PolygonGeofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.RectangleGeofence", b =>
                {
                    b.Property<long>("ID")
                        .HasColumnType("bigint");

                    b.Property<float>("East")
                        .HasColumnType("real");

                    b.Property<long>("GeofenceID")
                        .HasColumnType("bigint");

                    b.Property<float>("North")
                        .HasColumnType("real");

                    b.Property<float>("South")
                        .HasColumnType("real");

                    b.Property<float>("West")
                        .HasColumnType("real");

                    b.HasKey("ID");

                    b.ToTable("RectangleGeofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.RouteHistory", b =>
                {
                    b.Property<long>("RouteHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("RouteHistoryID"));

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<long>("Epoch")
                        .HasColumnType("bigint");

                    b.Property<int>("Latitude")
                        .HasColumnType("integer");

                    b.Property<int>("Longitude")
                        .HasColumnType("integer");

                    b.Property<char>("Status")
                        .HasColumnType("character(1)");

                    b.Property<int>("VehicleDirection")
                        .HasColumnType("integer");

                    b.Property<long>("VehicleID")
                        .HasColumnType("bigint");

                    b.Property<string>("VehicleSpeed")
                        .HasColumnType("text");

                    b.HasKey("RouteHistoryID");

                    b.HasIndex("VehicleID");

                    b.ToTable("RouteHistories");
                });

            modelBuilder.Entity("Fleet_Management.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("UserId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("passwordhash");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer")
                        .HasColumnName("RoleId");

                    b.Property<string>("Username")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("username");

                    b.HasKey("UserId")
                        .HasName("User_pkey");

                    b.HasIndex("RoleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("Fleet_Management.Models.UserRole", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("RoleId");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("rolename");

                    b.HasKey("RoleId")
                        .HasName("UserRole_pkey");

                    b.ToTable("UserRole", (string)null);
                });

            modelBuilder.Entity("Fleet_Management.Models.Vehicle", b =>
                {
                    b.Property<long>("VehicleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("VehicleID"));

                    b.Property<long>("VehicleNumber")
                        .HasColumnType("bigint");

                    b.Property<string>("VehicleType")
                        .HasColumnType("text");

                    b.HasKey("VehicleID");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("Fleet_Management.Models.VehicleInformation", b =>
                {
                    b.Property<long>("VehicleInformationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("VehicleInformationID"));

                    b.Property<long>("DriverID")
                        .HasColumnType("bigint");

                    b.Property<long>("PurchaseDate")
                        .HasColumnType("bigint");

                    b.Property<long>("VehicleID")
                        .HasColumnType("bigint");

                    b.Property<string>("VehicleMake")
                        .HasColumnType("text");

                    b.Property<string>("VehicleModel")
                        .HasColumnType("text");

                    b.HasKey("VehicleInformationID");

                    b.HasIndex("DriverID");

                    b.HasIndex("VehicleID")
                        .IsUnique();

                    b.ToTable("VehicleInformations");
                });

            modelBuilder.Entity("Fleet_Management.Models.CircleGeofence", b =>
                {
                    b.HasOne("Fleet_Management.Models.Geofence", "Geofence")
                        .WithOne("CircleGeofence")
                        .HasForeignKey("Fleet_Management.Models.CircleGeofence", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Geofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.PolygonGeofence", b =>
                {
                    b.HasOne("Fleet_Management.Models.Geofence", "Geofence")
                        .WithMany("PolygonGeofences")
                        .HasForeignKey("GeofenceID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Geofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.RectangleGeofence", b =>
                {
                    b.HasOne("Fleet_Management.Models.Geofence", "Geofence")
                        .WithOne("RectangleGeofence")
                        .HasForeignKey("Fleet_Management.Models.RectangleGeofence", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Geofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.RouteHistory", b =>
                {
                    b.HasOne("Fleet_Management.Models.Vehicle", "Vehicle")
                        .WithMany("RouteHistories")
                        .HasForeignKey("VehicleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Fleet_Management.Models.User", b =>
                {
                    b.HasOne("Fleet_Management.Models.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_User_UserRole");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Fleet_Management.Models.VehicleInformation", b =>
                {
                    b.HasOne("Fleet_Management.Models.Driver", "Driver")
                        .WithMany("VehicleInformations")
                        .HasForeignKey("DriverID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Fleet_Management.Models.Vehicle", "Vehicle")
                        .WithOne("VehicleInformation")
                        .HasForeignKey("Fleet_Management.Models.VehicleInformation", "VehicleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Vehicle");
                });

            modelBuilder.Entity("Fleet_Management.Models.Driver", b =>
                {
                    b.Navigation("VehicleInformations");
                });

            modelBuilder.Entity("Fleet_Management.Models.Geofence", b =>
                {
                    b.Navigation("CircleGeofence");

                    b.Navigation("PolygonGeofences");

                    b.Navigation("RectangleGeofence");
                });

            modelBuilder.Entity("Fleet_Management.Models.UserRole", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Fleet_Management.Models.Vehicle", b =>
                {
                    b.Navigation("RouteHistories");

                    b.Navigation("VehicleInformation");
                });
#pragma warning restore 612, 618
        }
    }
}
