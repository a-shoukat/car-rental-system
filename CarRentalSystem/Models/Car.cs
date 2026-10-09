namespace CarRentalSystem.Models
{
    // Car inherits from Vehicle (INHERITANCE)
    public class Car : Vehicle
    {
        public decimal RentPerDay { get; set; }
        public bool IsAvailable { get; set; }

        public Car() { IsAvailable = true; }

        public Car(int id, string make, string model, int year, string color,
                   string licensePlate, decimal rentPerDay)
            : base(id, make, model, year, color, licensePlate)
        {
            RentPerDay = rentPerDay;
            IsAvailable = true;
        }

        // Overridden method (POLYMORPHISM)
        public override string GetDescription()
        {
            string status = IsAvailable ? "Available" : "Rented";
            return $"{base.GetDescription()} - Rs {RentPerDay}/day [{status}]";
        }
    }
}
