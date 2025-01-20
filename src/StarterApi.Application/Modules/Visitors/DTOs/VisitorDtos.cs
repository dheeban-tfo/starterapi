using System;
using System.ComponentModel.DataAnnotations;
using StarterApi.Application.Common.Models;

namespace StarterApi.Application.Modules.Visitors.DTOs
{
    public class VisitorListDto
    {
        public Guid Id { get; set; }
        public string VisitorName { get; set; }
        public DateTime ExpectedVisitDate { get; set; }
        public TimeSpan ExpectedVisitStartTime { get; set; }
        public TimeSpan ExpectedVisitEndTime { get; set; }
        public string Status { get; set; }
        public string ResidentName { get; set; }
        public bool IsParking { get; set; }
    }

    public class VisitorDto
    {
        public Guid Id { get; set; }
        public string VisitorName { get; set; }
        public DateTime ExpectedVisitDate { get; set; }
        public TimeSpan ExpectedVisitStartTime { get; set; }
        public TimeSpan ExpectedVisitEndTime { get; set; }
        public string PurposeOfVisit { get; set; }
        public string Status { get; set; }
        public bool IsParking { get; set; }
        public LookupDetailDto SelectedResident { get; set; }
    }

    public class CreateVisitorDto
    {
        [Required]
        [StringLength(100)]
        public string VisitorName { get; set; }

        [Required]
        public DateTime ExpectedVisitDate { get; set; }

        [Required]
        public TimeSpan ExpectedVisitStartTime { get; set; }

        [Required]
        public TimeSpan ExpectedVisitEndTime { get; set; }

        [Required]
        [StringLength(500)]
        public string PurposeOfVisit { get; set; }

        public bool IsParking { get; set; }
    }

    public class UpdateVisitorDto
    {
        [Required]
        [StringLength(100)]
        public string VisitorName { get; set; }

        [Required]
        public DateTime ExpectedVisitDate { get; set; }

        [Required]
        public TimeSpan ExpectedVisitStartTime { get; set; }

        [Required]
        public TimeSpan ExpectedVisitEndTime { get; set; }

        [Required]
        [StringLength(500)]
        public string PurposeOfVisit { get; set; }

        public string Status { get; set; }

        public bool IsParking { get; set; }
    }

    public class UpdateVisitorStatusDto
    {
        [Required]
        public string Status { get; set; }
    }
} 