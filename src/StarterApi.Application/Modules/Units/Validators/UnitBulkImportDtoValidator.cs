using FluentValidation;
using StarterApi.Application.Modules.Units.DTOs;

namespace StarterApi.Application.Modules.Units.Validators
{
    public class UnitBulkImportDtoValidator : AbstractValidator<UnitBulkImportDto>
    {
        public UnitBulkImportDtoValidator()
        {
            RuleFor(x => x.BlockCode)
                .NotEmpty().WithMessage("Block code is required")
                .MaximumLength(50).WithMessage("Block code cannot exceed 50 characters");

            RuleFor(x => x.BlockName)
                .NotEmpty().WithMessage("Block name is required")
                .MaximumLength(100).WithMessage("Block name cannot exceed 100 characters");

            RuleFor(x => x.FloorNumber)
                .GreaterThan(0).WithMessage("Floor number must be greater than 0");

            RuleFor(x => x.FloorName)
                .NotEmpty().WithMessage("Floor name is required")
                .MaximumLength(100).WithMessage("Floor name cannot exceed 100 characters");

            RuleFor(x => x.UnitNumber)
                .NotEmpty().WithMessage("Unit number is required")
                .MaximumLength(50).WithMessage("Unit number cannot exceed 50 characters");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Unit type is required")
                .MaximumLength(50).WithMessage("Unit type cannot exceed 50 characters");

            RuleFor(x => x.BuiltUpArea)
                .GreaterThan(0).WithMessage("Built-up area must be greater than 0");

            RuleFor(x => x.CarpetArea)
                .GreaterThan(0).WithMessage("Carpet area must be greater than 0")
                .LessThanOrEqualTo(x => x.BuiltUpArea).WithMessage("Carpet area cannot be greater than built-up area");

            RuleFor(x => x.FurnishingStatus)
                .NotEmpty().WithMessage("Furnishing status is required")
                .MaximumLength(50).WithMessage("Furnishing status cannot exceed 50 characters");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters");

            RuleFor(x => x.MonthlyMaintenanceFee)
                .GreaterThanOrEqualTo(0).WithMessage("Monthly maintenance fee must be greater than or equal to 0");
        }
    }
} 