namespace StarterApi.Application.Modules.Units.DTOs
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public Guid FloorId { get; set; }
        public string UnitNumber { get; set; }
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string FurnishingStatus { get; set; }
        public string Status { get; set; }
        public Guid? CurrentOwnerId { get; set; }
        public decimal MonthlyMaintenanceFee { get; set; }
        public string FloorName { get; set; }
        public string BlockName { get; set; }
        public string BlockCode { get; set; }
        public string CurrentOwnerName { get; set; }
    }

    public class CreateUnitDto
    {
        public Guid FloorId { get; set; }
        public string UnitNumber { get; set; }
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string FurnishingStatus { get; set; }
        public string Status { get; set; }
        public Guid? CurrentOwnerId { get; set; }
    }

    public class UpdateUnitDto
    {
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string FurnishingStatus { get; set; }
        public string Status { get; set; }
        public Guid? CurrentOwnerId { get; set; }
    }
}
