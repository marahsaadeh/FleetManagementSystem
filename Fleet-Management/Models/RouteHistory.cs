using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class RouteHistory
    {
        [Key]
        public long RouteHistoryID { get; set; }
        [ForeignKey("Vehicle")]
        public long VehicleID { get; set; }
        public int VehicleDirection { get; set; }
        public char Status { get; set; }
        public string? VehicleSpeed { get; set; }
        public string? RecordTime { get; set; }
        public string? Address { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }

        // Navigation properties
        public virtual Vehicle? Vehicle { get; set; }
    }

}