using FluentValidation;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Validators.MotorcycleValidators
{
    public class GetMotorcyclePerPlateDTOValidator : AbstractValidator<GetMotorcyclePerPlateDTO>
    {
        public GetMotorcyclePerPlateDTOValidator()
        {
            RuleFor(m => m.Plate)
                .NotEmpty()
                .WithMessage("Plate is required");

        }
    }
}
