using System;

namespace StarterApi.Application.Modules.Units.DTOs
{
    public class UnitListDto
    {
        public Guid Id { get; set; }
        public string UnitNumber { get; set; }
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public string Status { get; set; }
        public string FloorName { get; set; }
        public string BlockName { get; set; }
        public string CurrentOwnerName { get; set; }
    }
}
