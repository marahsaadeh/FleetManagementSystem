using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fleet_Management.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DriverName = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverID);
                });

            migrationBuilder.CreateTable(
                name: "Geofences",
                columns: table => new
                {
                    GeofenceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeofenceType = table.Column<string>(type: "text", nullable: true),
                    AddedDate = table.Column<long>(type: "bigint", nullable: false),
                    StrokeColor = table.Column<string>(type: "text", nullable: true),
                    StrokeOpacity = table.Column<int>(type: "integer", nullable: false),
                    StrokeWeight = table.Column<int>(type: "integer", nullable: false),
                    FillColor = table.Column<string>(type: "text", nullable: true),
                    FillOpacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geofences", x => x.GeofenceID);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleNumber = table.Column<long>(type: "bigint", nullable: false),
                    VehicleType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleID);
                });

            migrationBuilder.CreateTable(
                name: "CircleGeofence",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    Radius = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CircleGeofence", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CircleGeofence_Geofences_ID",
                        column: x => x.ID,
                        principalTable: "Geofences",
                        principalColumn: "GeofenceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PolygonGeofence",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GeofenceID = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolygonGeofence", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PolygonGeofence_Geofences_GeofenceID",
                        column: x => x.GeofenceID,
                        principalTable: "Geofences",
                        principalColumn: "GeofenceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RectangleGeofence",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false),
                    North = table.Column<int>(type: "integer", nullable: false),
                    East = table.Column<int>(type: "integer", nullable: false),
                    West = table.Column<int>(type: "integer", nullable: false),
                    South = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RectangleGeofence", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RectangleGeofence_Geofences_ID",
                        column: x => x.ID,
                        principalTable: "Geofences",
                        principalColumn: "GeofenceID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RouteHistories",
                columns: table => new
                {
                    RouteHistoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleID = table.Column<long>(type: "bigint", nullable: false),
                    VehicleDirection = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<char>(type: "character(1)", nullable: false),
                    VehicleSpeed = table.Column<string>(type: "text", nullable: true),
                    RecordTime = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteHistories", x => x.RouteHistoryID);
                    table.ForeignKey(
                        name: "FK_RouteHistories_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInformations",
                columns: table => new
                {
                    VehicleInformationID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleID = table.Column<long>(type: "bigint", nullable: false),
                    DriverID = table.Column<long>(type: "bigint", nullable: false),
                    VehicleMake = table.Column<string>(type: "text", nullable: true),
                    VehicleModel = table.Column<string>(type: "text", nullable: true),
                    PurchaseDate = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInformations", x => x.VehicleInformationID);
                    table.ForeignKey(
                        name: "FK_VehicleInformations_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VehicleInformations_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PolygonGeofence_GeofenceID",
                table: "PolygonGeofence",
                column: "GeofenceID");

            migrationBuilder.CreateIndex(
                name: "IX_RouteHistories_VehicleID",
                table: "RouteHistories",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInformations_DriverID",
                table: "VehicleInformations",
                column: "DriverID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInformations_VehicleID",
                table: "VehicleInformations",
                column: "VehicleID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CircleGeofence");

            migrationBuilder.DropTable(
                name: "PolygonGeofence");

            migrationBuilder.DropTable(
                name: "RectangleGeofence");

            migrationBuilder.DropTable(
                name: "RouteHistories");

            migrationBuilder.DropTable(
                name: "VehicleInformations");

            migrationBuilder.DropTable(
                name: "Geofences");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
