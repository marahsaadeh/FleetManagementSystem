using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class RectangleGeofence
    {
        [Key]
        public long ID { get; set; }
        [ForeignKey("Geofence")]
        public long GeofenceID { get; set; }
        public float North { get; set; }
        public float East { get; set; }
        public float West { get; set; }
        public float South { get; set; }


        public virtual Geofence? Geofence { get; set; }
        


    }


}