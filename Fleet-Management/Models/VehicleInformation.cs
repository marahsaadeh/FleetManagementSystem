using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class VehicleInformation
    {
        [Key]
        public long VehicleInformationID { get; set; }

       
        [ForeignKey("Vehicle")]
        public long VehicleID { get; set; }
        public virtual Vehicle? Vehicle { get; set; }

   
        [ForeignKey("Driver")]
        public long DriverID { get; set; }
        public virtual Driver? Driver { get; set; }

        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public long PurchaseDate { get; set; }

      
    }

}


