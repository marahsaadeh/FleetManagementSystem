using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class CircleGeofence
    {
        [Key]
        public long ID { get; set; }
        [ForeignKey("Geofence")]
        public long GeofenceID { get; set; }
        public long Radius { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public virtual Geofence? Geofence { get; set; }
    }

}