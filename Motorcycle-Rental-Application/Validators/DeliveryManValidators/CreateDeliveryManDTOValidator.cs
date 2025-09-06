using FluentValidation;
using Motorcycle_Rental_Application.DTOs.DeliveryManDTO;

namespace Motorcycle_Rental_Application.Validators.DeliveryManValidators
{
    public class CreateDeliveryManDTOValidator : AbstractValidator<CreateDeliveryManDTO>
    {
        public CreateDeliveryManDTOValidator()
        {
            RuleFor(x => x.Identifier).NotEmpty().WithMessage("Identifier is required");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Nome é obrigatório");
            RuleFor(x => x.CNPJ).NotEmpty().WithMessage("CNPJ é obrigatório").Length(14).WithMessage("CNPJ deve ter 14 caracteres");
            RuleFor(x => x.CNHNumber).NotEmpty().WithMessage("CNH é obrigatória");
            RuleFor(x => x.CNHType)
                .Must(t => t == "A" || t == "B" || t == "A+B")
                .WithMessage("Tipo de CNH deve ser A, B ou A+B");
        }
    
    }
}
