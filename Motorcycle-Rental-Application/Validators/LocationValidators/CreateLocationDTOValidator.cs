using FluentValidation;
using Motorcycle_Rental_Application.DTOs.LocationDTO;

namespace Motorcycle_Rental_Application.Validators.LocationValidators
{

    public class CreateLocationDTOValidator : AbstractValidator<CreateLocationDTO>
    {
        public CreateLocationDTOValidator()
        {
            RuleFor(l => l.DeliveryMan_Id)
            .NotEmpty().NotNull()
            .WithMessage("Identifier is required.");

            RuleFor(l => l.Motorcycle_Id)
            .NotEmpty().NotNull()
            .WithMessage("Identifier is required.");

            RuleFor(l => l.StartDate)
            .NotEmpty().NotNull()
            .WithMessage("Identifier is required.");

            RuleFor(l => l.EndDate)
            .NotEmpty().NotNull()
            .WithMessage("Identifier is required.");

            RuleFor(l => l.EstimatedEndDate)
            .NotEmpty().NotNull()
            .WithMessage("Identifier is required.");


            RuleFor(l => l.Plan)
            .NotEmpty().NotNull()
            .WithMessage("Identifier is required.");

        }
    }
}
