using System;

namespace StarterApi.Application.Modules.Societies.DTOs
{
    public class SocietyListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int BlockCount { get; set; }
        public int UnitCount { get; set; }
    }
}
