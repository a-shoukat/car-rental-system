namespace CarRentalSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string LicenseNumber { get; set; }  // driving license number

        public Customer() { }

        public Customer(int id, string name, string phone, string licenseNumber)
        {
            Id = id;
            Name = name;
            Phone = phone;
            LicenseNumber = licenseNumber;
        }

        public override string ToString()
        {
            return $"{Name} ({Phone})";
        }
    }
}
