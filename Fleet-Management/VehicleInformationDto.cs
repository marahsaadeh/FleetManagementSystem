
    namespace Fleet_Management
    {
    public class VehicleInformationDto
    {
        public long VehicleInformationID { get; set; }
        public long VehicleID { get; set; }
        public long VehicleNumber { get; set; } 
        public string? VehicleType { get; set; }
        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public DateTime PurchaseDate { get; set; }
        public long DriverID { get; set; }
        public string? DriverName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LastGPSSpeed { get; set; }
        public string? LastAddress { get; set; }
        public DateTime? LastGPSTime { get; set; }
        public string? LastPosition { get; set; }
    }
}

