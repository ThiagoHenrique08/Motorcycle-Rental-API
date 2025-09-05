using FluentValidation;
using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;
using Motorcycle_Rental_Domain.Models;

namespace Motorcycle_Rental_Application.Validators.MotorcycleValidators
{
    public class CreateMotorcycleDTOValidator : AbstractValidator<CreateMotorcycleDTO>
    {
        public CreateMotorcycleDTOValidator()
        {
            // Identifier obrigatório
            RuleFor(m => m.Identifier)
                .NotEmpty().WithMessage("Identifier is required.");

            // Modelo obrigatório
            RuleFor(m => m.Model)
                .NotEmpty().WithMessage("Model is required.");
              

            // Ano deve ser maior que 2000 e não pode ser no futuro
            RuleFor(m => m.Year)
                .NotEmpty().WithMessage("Year is required.")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("The year of the motorcycle cannot be in the future.");

            // Placa obrigatória (simples validação de tamanho)
            RuleFor(m => m.Plate)
                .NotEmpty().WithMessage("Plate is required.")
                .Length(8).WithMessage("The license plate must have 8 characters.");


        }
    }
}

