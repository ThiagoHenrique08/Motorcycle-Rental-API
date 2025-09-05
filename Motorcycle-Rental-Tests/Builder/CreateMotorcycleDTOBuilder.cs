using Motorcycle_Rental_Application.DTOs.MotorcycleDTO;

namespace Motorcycle_Rental_Tests.Builder
{
    internal class CreateMotorcycleDTOBuilder
    {
        public string Identifier { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public CreateMotorcycleDTOBuilder()
        {
            Identifier = "Moto123";
            Year = 2025;
            Model = "Sport";
            Plate = "CDX-0101";
        }

        public CreateMotorcycleDTOBuilder WithIdentifier(string identifier)
        {
            Identifier = identifier;
            return this;
        }
        public CreateMotorcycleDTOBuilder WithYear(int year)
        {
            Year = year;
            return this;
        }
        public CreateMotorcycleDTOBuilder WithModel(string model)
        {
            Model = model;
            return this;
        }

        public CreateMotorcycleDTOBuilder WithPlate(string plate)
        {
            Plate = plate;
            return this;
        }

        public CreateMotorcycleDTO Build()
        {
            return new CreateMotorcycleDTO(Identifier, Year, Model, Plate);
        
        }
    }
}
