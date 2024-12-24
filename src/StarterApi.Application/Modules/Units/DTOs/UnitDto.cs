using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Units.DTOs
{
    public class UnitDto
    {
        public Guid Id { get; set; }
        public Guid FloorId { get; set; }
        public string UnitNumber { get; set; }
        public string Type { get; set; }
        public decimal BuiltUpArea { get; set; }
        public decimal CarpetArea { get; set; }
        public string FurnishingStatus { get; set; }
        public string Status { get; set; }
        public Guid? CurrentOwnerId { get; set; }
        public decimal MonthlyMaintenanceFee { get; set; }
        public string FloorName { get; set; }
        public int FloorNumber { get; set; }
        public string BlockName { get; set; }
        public string BlockCode { get; set; }
        public string SocietyName { get; set; }
    }

    public class CreateUnitDto
    {
        [Required]
        public Guid FloorId { get; set; }

        [Required]
        [StringLength(50)]
        public string UnitNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BuiltUpArea { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CarpetArea { get; set; }

        [Required]
        [StringLength(50)]
        public string FurnishingStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public Guid? CurrentOwnerId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlyMaintenanceFee { get; set; }
    }

    public class UpdateUnitDto
    {
        [Required]
        [StringLength(50)]
        public string Type { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BuiltUpArea { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal CarpetArea { get; set; }

        [Required]
        [StringLength(50)]
        public string FurnishingStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public Guid? CurrentOwnerId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlyMaintenanceFee { get; set; }
    }
}
