using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class RectangleGeofence
    {
        [Key]
        [ForeignKey("Geofence")]
        public long ID { get; set; }
        public int North { get; set; }
        public int East { get; set; }
        public int West { get; set; }
        public int South { get; set; }

        // Navigation property
        public virtual Geofence? Geofence { get; set; }
    }


}