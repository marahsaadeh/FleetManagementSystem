using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mono.TextTemplating;

namespace Fleet_Management.Models
{
    public class RouteHistory
    {
        [Key]
        public long RouteHistoryID { get; set; }
        [ForeignKey("Vehicle")]
        public long VehicleID { get; set; }
        public int VehicleDirection { get; set; }
        public char? Status { get; set; }
        public string? VehicleSpeed { get; set; }
        public long Epoch { get; set; }
        public string? Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public virtual Vehicle? Vehicle { get; set; }
     

    }

}