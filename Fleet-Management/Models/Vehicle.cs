using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleet_Management.Models
{
    public class Vehicle
    {
        [Key]
        public long VehicleID { get; set; }
        public long VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        // علاقة واحد إلى كثير مع RouteHistory
        public virtual ICollection<RouteHistory>? RouteHistories { get; set; }

        // علاقة واحد إلى واحد مع VehicleInformation
        public virtual VehicleInformation? VehicleInformation { get; set; }
       
    }

    


}

