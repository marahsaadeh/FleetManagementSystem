using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class CircleGeofence
    {
        [Key]
        [ForeignKey("Geofence")]
        public long ID { get; set; }
        public long Radius { get; set; }
        public int Latitude { get; set; }
        public int Longitude { get; set; }

        // Navigation property
        public virtual Geofence? Geofence { get; set; }
    }

}