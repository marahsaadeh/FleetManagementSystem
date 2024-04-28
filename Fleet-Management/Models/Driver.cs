using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class Driver
    {
        [Key]
        public long DriverID { get; set; }
        public string? DriverName { get; set; }
        public long PhoneNumber { get; set; }

        // If each driver has a list of VehicleInformation, then keep this
         public virtual ICollection<VehicleInformation>? VehicleInformations { get; set; }
    }



}