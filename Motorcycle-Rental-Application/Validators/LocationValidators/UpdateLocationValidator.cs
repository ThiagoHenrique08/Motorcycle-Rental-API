using FluentValidation;
using Motorcycle_Rental_Application.DTOs.LocationDTO;

namespace Motorcycle_Rental_Application.Validators.LocationValidators
{
    public class UpdateLocationValidator : AbstractValidator<UpdateLocationDTO>
    {
        public UpdateLocationValidator()
        {
            RuleFor(location => location.ReturnDate).NotEmpty().NotNull().WithMessage("Data de devolução obrigatória");
        }
    }
}
