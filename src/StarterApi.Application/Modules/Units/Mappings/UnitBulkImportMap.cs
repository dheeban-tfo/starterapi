using CsvHelper.Configuration;
using StarterApi.Application.Modules.Units.DTOs;

namespace StarterApi.Application.Modules.Units.Mappings
{
    public sealed class UnitBulkImportMap : ClassMap<UnitBulkImportDto>
    {
        public UnitBulkImportMap()
        {
            Map(m => m.BlockCode).Name("BlockCode");
            Map(m => m.BlockName).Name("BlockName");
            Map(m => m.FloorNumber).Name("FloorNumber");
            Map(m => m.FloorName).Name("FloorName");
            Map(m => m.UnitNumber).Name("UnitNumber");
            Map(m => m.Type).Name("Type");
            Map(m => m.BuiltUpArea).Name("BuiltUpArea");
            Map(m => m.CarpetArea).Name("CarpetArea");
            Map(m => m.FurnishingStatus).Name("FurnishingStatus");
            Map(m => m.Status).Name("Status");
            Map(m => m.MonthlyMaintenanceFee).Name("MonthlyMaintenanceFee");
        }
    }
} 