using System;

namespace StarterApi.Domain.Common
{
    public abstract class LookupDetailEntity : BaseEntity
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
