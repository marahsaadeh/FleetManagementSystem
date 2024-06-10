using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class Driver
    {
        [Key]
        public long DriverID { get; set; }
        public string? DriverName { get; set; }
        public long PhoneNumber { get; set; }


         public virtual ICollection<VehicleInformation>? VehicleInformations { get; set; }
    }



}