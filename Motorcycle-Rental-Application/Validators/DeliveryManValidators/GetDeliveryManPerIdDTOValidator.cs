using FluentValidation;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Application.Validators.DeliveryManValidators
{
    public class GetDeliveryManPerIdDTOValidator : AbstractValidator<GetDeliveryManPerIdDTO>
    {

        public GetDeliveryManPerIdDTOValidator()
        {
            RuleFor(id => id.Id)
             .NotEmpty()
             .WithMessage("Id is required");
        }
    }
}
