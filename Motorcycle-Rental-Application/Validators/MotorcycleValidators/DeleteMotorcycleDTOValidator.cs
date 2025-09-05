using FluentValidation;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Validators.MotorcycleValidators
{
    public class DeleteMotorcycleDTOValidator : AbstractValidator<DeleteMotorcycleDTO>
    {
        public DeleteMotorcycleDTOValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }
}
