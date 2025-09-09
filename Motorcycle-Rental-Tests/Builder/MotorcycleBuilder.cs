namespace Motorcycle_Rental_Tests.Builder
{
    public class MotorcycleBuilder
    {

        public string Identifier { get; set; }
        public int Year { get; set; }
        public string Model { get; set; }
        public string Plate { get; set; }

        public MotorcycleBuilder()
        {
            Identifier = "Moto123";
            Year = 2025;
            Model = "Sport";
            Plate = "CDX-0101";
        }

        public MotorcycleBuilder WithIdentifier(string identifier)
        {
            Identifier = identifier;
            return this;
        }
        public MotorcycleBuilder WithYear(int year)
        {
            Year = year;
            return this;
        }
        public MotorcycleBuilder WithModel(string model)
        {
            Model = model;
            return this;
        }

        public MotorcycleBuilder WithPlate(string plate)
        {
            Plate = plate;
            return this;
        }

        public Motorcycle_Rental_Domain.Models.Motorcycle Build()
        {
            return new Motorcycle_Rental_Domain.Models.Motorcycle
            {
                Identifier = this.Identifier,
                Model = this.Model,
                Year = this.Year,
                Plate = this.Plate
            };
        }
    }

}
