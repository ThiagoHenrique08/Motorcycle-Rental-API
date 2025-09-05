using FluentValidation;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Validators.MotorcycleValidators
{
    public class GetMotorcyclePerIdDTOValidator : AbstractValidator<string>
    {

        public GetMotorcyclePerIdDTOValidator()
        {
            RuleFor(id => id)
             .NotEmpty()
             .WithMessage("Id is required");
        }
    }
}
