using System.ComponentModel.DataAnnotations;

namespace Fleet_Management.Models
{
    public class Geofence
    {
        [Key]
        public long GeofenceID { get; set; }
        public string? GeofenceType { get; set; }
        public long AddedDate { get; set; }
        public string? StrokeColor { get; set; }
        public int StrokeOpacity { get; set; }
        public int StrokeWeight { get; set; }
        public string? FillColor { get; set; }
        public int FillOpacity { get; set; }

        // Navigation properties for different geofence shapes
        public virtual CircleGeofence? CircleGeofence { get; set; }
        public virtual RectangleGeofence? RectangleGeofence { get; set; }
        public virtual ICollection<PolygonGeofence>? PolygonGeofences { get; set; }
    }


}