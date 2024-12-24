namespace StarterApi.Application.Modules.Blocks.DTOs
{
    public class BlockDto
    {
        public Guid Id { get; set; }
        public Guid SocietyId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int TotalFloors { get; set; }
        public decimal MaintenanceChargePerSqft { get; set; }
        public string SocietyName { get; set; }
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
