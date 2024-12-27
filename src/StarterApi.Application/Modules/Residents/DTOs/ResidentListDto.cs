using System;

namespace StarterApi.Application.Modules.Residents.DTOs
{
    public class ResidentListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UnitNumber { get; set; }
        public string ResidentType { get; set; }
        public string Status { get; set; }
        public bool IsVerified { get; set; }
    }
}
