namespace CarRentalSystem.Models
{
    // Base class demonstrating INHERITANCE
    // Car inherits all of these common vehicle properties.
    public class Vehicle
    {
        public int Id { get; set; }
        public string Make { get; set; }      // e.g. Toyota, Honda
        public string Model { get; set; }     // e.g. Corolla, Civic
        public int Year { get; set; }
        public string Color { get; set; }
        public string LicensePlate { get; set; }

        public Vehicle() { }

        public Vehicle(int id, string make, string model, int year,
                       string color, string licensePlate)
        {
            Id = id;
            Make = make;
            Model = model;
            Year = year;
            Color = color;
            LicensePlate = licensePlate;
        }

        // Virtual method - can be overridden by child classes (POLYMORPHISM)
        public virtual string GetDescription()
        {
            return $"{Year} {Make} {Model} ({LicensePlate})";
        }
    }
}
