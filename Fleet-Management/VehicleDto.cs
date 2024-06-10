namespace Fleet_Management
{
    public class VehicleDto
    {
        public long VehicleID { get; set; }
        public long VehicleNumber { get; set; } 
        public string? VehicleType { get; set; }
        public int LastDirection { get; set; }
        public char? LastStatus { get; set; }
        public string? LastAddress { get; set; }
        public string? LastPosition { get; set; }
  
      
    }
}
