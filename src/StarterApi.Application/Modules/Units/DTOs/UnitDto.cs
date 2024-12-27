using StarterApi.Application.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Units.DTOs
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public string UnitNumber { get; set; }
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string FurnishingStatus { get; set; }
        public string Status { get; set; }
        public decimal MonthlyMaintenanceFee { get; set; }

        // Lookup fields with detailed information
        public FloorDetailDto SelectedFloor { get; set; }
        public BlockDetailDto SelectedBlock { get; set; }
        public SocietyDetailDto SelectedSociety { get; set; }
        public OwnerDetailDto SelectedCurrentOwner { get; set; }
    }

    public class CreateUnitDto
    {
        [Required]
        public Guid FloorId { get; set; }

        [Required]
        [StringLength(20)]
        public string UnitNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public decimal BuiltUpArea { get; set; }

        [Required]
        public decimal CarpetArea { get; set; }

        [Required]
        [StringLength(50)]
        public string FurnishingStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public Guid? CurrentOwnerId { get; set; }

        [Required]
        public decimal MonthlyMaintenanceFee { get; set; }
    }

    public class UpdateUnitDto
    {
        [Required]
        [StringLength(20)]
        public string UnitNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        public decimal BuiltUpArea { get; set; }

        [Required]
        public decimal CarpetArea { get; set; }

        [Required]
        [StringLength(50)]
        public string FurnishingStatus { get; set; }

        public Guid? CurrentOwnerId { get; set; }

        [Required]
        public decimal MonthlyMaintenanceFee { get; set; }
    }
}
