namespace StarterApi.Application.Modules.Floors.DTOs
{
    public class FloorDto
    {
        public Guid Id { get; set; }
        public Guid BlockId { get; set; }
        public int FloorNumber { get; set; }
        public string FloorName { get; set; }
        public int TotalUnits { get; set; }
        public string BlockName { get; set; }
        public string BlockCode { get; set; }
    }

    public class CreateFloorDto
    {
        public Guid BlockId { get; set; }
        public int FloorNumber { get; set; }
        public string FloorName { get; set; }
    }

    public class UpdateFloorDto
    {
        public string FloorName { get; set; }
    }
}
