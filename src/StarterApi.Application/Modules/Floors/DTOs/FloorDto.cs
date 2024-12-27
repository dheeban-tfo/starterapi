using StarterApi.Application.Common.Models;
using System;

namespace StarterApi.Application.Modules.Floors.DTOs
{
    public class FloorDto
    {
        public Guid Id { get; set; }
        public string FloorName { get; set; }
        public int FloorNumber { get; set; }
        public BlockDetailDto SelectedBlock { get; set; }
        public int UnitCount { get; set; }
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
