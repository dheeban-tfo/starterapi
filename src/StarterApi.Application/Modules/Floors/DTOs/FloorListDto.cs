using System;

namespace StarterApi.Application.Modules.Floors.DTOs
{
    public class FloorListDto
    {
        public Guid Id { get; set; }
        public string FloorName { get; set; }
        public int FloorNumber { get; set; }
        public string BlockName { get; set; }
        public int UnitCount { get; set; }
    }
}
