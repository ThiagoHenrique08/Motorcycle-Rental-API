using FluentValidation;

namespace Motorcycle_Rental_Application.Validators.LocationValidators
{
    public class GetLocationByIdDTOValidator : AbstractValidator<string>
    {
        public GetLocationByIdDTOValidator()
        {
            RuleFor(id => id).NotEmpty().NotNull().WithMessage("Id is required");
        }
    }
}
