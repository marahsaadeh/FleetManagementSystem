using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class PolygonGeofence
    {
        [Key]
        public long ID { get; set; }
        [ForeignKey("Geofence")]
        public long GeofenceID { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }

        // Navigation property
        public virtual Geofence? Geofence { get; set; }
    }


}