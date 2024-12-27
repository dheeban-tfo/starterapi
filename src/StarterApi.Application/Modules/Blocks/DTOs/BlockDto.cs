using StarterApi.Application.Common.Models;
using System;

namespace StarterApi.Application.Modules.Blocks.DTOs
{
    public class BlockDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public SocietyDetailDto SelectedSociety { get; set; }
        public int FloorCount { get; set; }
        public int UnitCount { get; set; }
        public decimal MaintenanceChargePerSqft { get; set; }
    }

    public class CreateBlockDto
    {
        public Guid SocietyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal MaintenanceChargePerSqft { get; set; }
    }

    public class UpdateBlockDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal MaintenanceChargePerSqft { get; set; }
    }
}
