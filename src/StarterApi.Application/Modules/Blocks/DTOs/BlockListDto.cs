using System;

namespace StarterApi.Application.Modules.Blocks.DTOs
{
    public class BlockListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SocietyName { get; set; }
        public int FloorCount { get; set; }
        public int UnitCount { get; set; }
    }
}
