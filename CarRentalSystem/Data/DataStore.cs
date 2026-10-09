using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using CarRentalSystem.Models;

namespace CarRentalSystem.Data
{
    // Central data manager: keeps all lists in memory and
    // saves/loads them as JSON files (simple file-based database).
    public static class DataStore
    {
        private static readonly string Folder =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        public static List<Car> Cars { get; private set; } = new List<Car>();
        public static List<Customer> Customers { get; private set; } = new List<Customer>();
        public static List<Rental> Rentals { get; private set; } = new List<Rental>();

        public static void Load()
        {
            Directory.CreateDirectory(Folder);
            Cars = Read<List<Car>>("cars.json") ?? new List<Car>();
            Customers = Read<List<Customer>>("customers.json") ?? new List<Customer>();
            Rentals = Read<List<Rental>>("rentals.json") ?? new List<Rental>();

            // First run: add a few sample cars so the app isn't empty
            if (Cars.Count == 0)
            {
                Cars.Add(new Car(1, "Toyota", "Corolla", 2022, "White", "LEA-1234", 5000));
                Cars.Add(new Car(2, "Honda", "Civic", 2023, "Black", "LEB-5678", 7000));
                Cars.Add(new Car(3, "Suzuki", "Swift", 2021, "Silver", "LEC-9012", 4000));
                Save();
            }
        }

        public static void Save()
        {
            Directory.CreateDirectory(Folder);
            Write("cars.json", Cars);
            Write("customers.json", Customers);
            Write("rentals.json", Rentals);
        }

        private static T Read<T>(string fileName)
        {
            string path = Path.Combine(Folder, fileName);
            if (!File.Exists(path)) return default;
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<T>(json);
        }

        private static void Write<T>(string fileName, T data)
        {
            string path = Path.Combine(Folder, fileName);
            string json = JsonSerializer.Serialize(data,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }

        public static int NextCarId() =>
            Cars.Count == 0 ? 1 : Cars.Max(c => c.Id) + 1;

        public static int NextCustomerId() =>
            Customers.Count == 0 ? 1 : Customers.Max(c => c.Id) + 1;

        public static int NextRentalId() =>
            Rentals.Count == 0 ? 1 : Rentals.Max(r => r.Id) + 1;
    }
}
