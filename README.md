# 🚗 Car Rental System

My **second-semester Object-Oriented Programming (OOP) project** — a complete car rental management application with a graphical interface, built in **C# with Windows Forms**.

## ✨ Features

- 🔐 **Login screen** — simple authentication (demo: `admin` / `admin`)
- 📊 **Dashboard** — live stats: total cars, available/rented counts, active rentals
- 🚗 **Car management** — add, edit, delete cars with rent-per-day pricing
- 👥 **Customer management** — add, edit, delete customers (protected if they have an active rental)
- 📝 **Rent a car** — pick an available car + customer, choose dates, auto-calculates total cost
- ↩️ **Return a car** — mark rentals returned; final bill recalculated on actual return date
- 💾 **Data persistence** — all data saved as JSON files, loaded automatically on startup

## 🧠 OOP Concepts Demonstrated

| Concept | Where |
|---|---|
| Classes & Objects | `Car`, `Customer`, `Rental` |
| Inheritance | `Car : Vehicle` (base class) |
| Polymorphism | `GetDescription()` overridden in `Car` |
| Encapsulation | Business logic (`CalculateCost`, `GetRentalDays`) inside `Rental` |
| File Handling | JSON persistence in `DataStore` |

## 📁 Project Structure

```
CarRentalSystem/
├── Program.cs                 # Application entry point
├── Models/
│   ├── Vehicle.cs             # Base class (inheritance demo)
│   ├── Car.cs                 # Inherits Vehicle
│   ├── Customer.cs
│   └── Rental.cs              # Rental logic & cost calculation
├── Data/
│   └── DataStore.cs           # JSON file persistence
└── Forms/
    ├── LoginForm.cs
    ├── MainForm.cs            # Dashboard
    ├── CarsForm.cs / CarEditForm.cs
    ├── CustomersForm.cs       # (includes CustomerEditForm)
    ├── RentForm.cs
    └── ReturnForm.cs
```

## ▶️ How to Run

**Option 1 — Visual Studio (recommended):**
1. Open `CarRentalSystem.sln` in Visual Studio 2022
2. Press `F5` to build and run

**Option 2 — .NET SDK:**
```bash
dotnet build
dotnet run --project CarRentalSystem
```
Requires .NET 8 SDK. Login with username `admin`, password `admin`.

## 🛠️ Tech

- C# (.NET 8)
- Windows Forms (WinForms)
- System.Text.Json for data storage

## 👩‍💻 Author

**Ayesha Shoukat** — Computer Science @ UET Narowal
