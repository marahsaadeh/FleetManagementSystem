using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleet_Management.Models
{
    public class Vehicle
    {
        [Key]
        public long VehicleID { get; set; }
        [Required]
        public long VehicleNumber { get; set; }
        [Required]
        public string? VehicleType { get; set; }
        
        public virtual ICollection<RouteHistory>? RouteHistories { get; set; }

        
        public virtual VehicleInformation? VehicleInformation { get; set; }
       
    }

    


}

