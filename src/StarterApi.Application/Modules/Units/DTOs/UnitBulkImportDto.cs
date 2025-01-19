using System.ComponentModel.DataAnnotations;

namespace StarterApi.Application.Modules.Units.DTOs
{
    public class UnitBulkImportDto
    {
        public Guid SocietyId { get; set; }
        [Required]
        public string BlockCode { get; set; }
        
        [Required]
        public string BlockName { get; set; }
        
        [Required]
        public int FloorNumber { get; set; }
        
        [Required]
        public string FloorName { get; set; }
        
        [Required]
        public string UnitNumber { get; set; }
        
        [Required]
        public string Type { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal BuiltUpArea { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal CarpetArea { get; set; }
        
        [Required]
        public string FurnishingStatus { get; set; }
        
        [Required]
        public string Status { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal MonthlyMaintenanceFee { get; set; }
    }

    public class UnitBulkImportResultDto
    {
        public int TotalProcessed { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
} 