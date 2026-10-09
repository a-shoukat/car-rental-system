using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarRentalSystem.Data;

namespace CarRentalSystem.Forms
{
    public class MainForm : Form
    {
        private Label lblStats;

        public MainForm()
        {
            Text = "Car Rental System - Dashboard";
            Size = new Size(620, 420);
            StartPosition = FormStartPosition.CenterScreen;

            var lblTitle = new Label
            {
                Text = "Car Rental System",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 42, 67),
                AutoSize = true,
                Location = new Point(30, 20)
            };

            var lblSub = new Label
            {
                Text = "Welcome, Admin! Choose an option below.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(32, 60)
            };

            lblStats = new Label
            {
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 118, 110),
                AutoSize = true,
                Location = new Point(32, 90)
            };

            string[] names = { "Manage Cars", "Manage Customers", "Rent a Car",
                               "Return a Car", "View All Rentals", "Logout" };
            EventHandler[] actions = { (s, e) => Open<CarsForm>(),
                                       (s, e) => Open<CustomersForm>(),
                                       (s, e) => Open<RentForm>(),
                                       (s, e) => Open<ReturnForm>(),
                                       (s, e) => Open<ReturnForm>(),
                                       (s, e) => Close() };

            for (int i = 0; i < names.Length; i++)
            {
                var btn = new Button
                {
                    Text = names[i],
                    Size = new Size(160, 55),
                    Location = new Point(32 + (i % 3) * 180, 130 + (i / 3) * 70),
                    BackColor = Color.FromArgb(13, 42, 67),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10)
                };
                btn.Click += actions[i];
                Controls.Add(btn);
            }

            Controls.AddRange(new Control[] { lblTitle, lblSub, lblStats });
            RefreshStats();
        }

        private void Open<T>() where T : Form, new()
        {
            using (var f = new T()) { f.ShowDialog(); }
            RefreshStats();
        }

        private void RefreshStats()
        {
            int available = DataStore.Cars.Count(c => c.IsAvailable);
            int rented = DataStore.Cars.Count(c => !c.IsAvailable);
            int active = DataStore.Rentals.Count(r => !r.IsReturned);
            lblStats.Text = $"Cars: {DataStore.Cars.Count}  |  Available: {available}  |  Rented: {rented}  |  Active rentals: {active}";
        }
    }
}
