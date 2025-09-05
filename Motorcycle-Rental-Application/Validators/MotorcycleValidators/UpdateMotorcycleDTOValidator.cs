using FluentValidation;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Validators.MotorcycleValidators
{
    public class UpdateMotorcycleDTOValidator : AbstractValidator<UpdateMotorcycleDTO>
    {

        public UpdateMotorcycleDTOValidator()
        {

            RuleFor(m => m.Plate)
                .NotEmpty().WithMessage("Plate is required.")
                .Length(8).WithMessage("The license plate must have 8 characters.");
        }
    }
}
