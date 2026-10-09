using System;

namespace CarRentalSystem.Models
{
    public class Rental
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public decimal TotalCost { get; set; }
        public bool IsReturned { get; set; }

        // Read-only property showing linked info (filled by the UI layer)
        public string CarInfo { get; set; }
        public string CustomerName { get; set; }

        public Rental() { }

        public Rental(int id, int carId, int customerId,
                      DateTime rentDate, DateTime expectedReturnDate)
        {
            Id = id;
            CarId = carId;
            CustomerId = customerId;
            RentDate = rentDate;
            ExpectedReturnDate = expectedReturnDate;
            IsReturned = false;
        }

        // Business logic lives inside the class (ENCAPSULATION)
        public int GetRentalDays()
        {
            DateTime end = IsReturned && ActualReturnDate.HasValue
                ? ActualReturnDate.Value
                : ExpectedReturnDate;
            int days = (end.Date - RentDate.Date).Days;
            return days < 1 ? 1 : days;   // minimum 1 day charge
        }

        public decimal CalculateCost(decimal rentPerDay)
        {
            return GetRentalDays() * rentPerDay;
        }
    }
}
