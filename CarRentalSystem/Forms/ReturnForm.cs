using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarRentalSystem.Data;
using CarRentalSystem.Models;

namespace CarRentalSystem.Forms
{
    public class ReturnForm : Form
    {
        private DataGridView grid;

        public ReturnForm()
        {
            Text = "Rentals - Return a Car";
            Size = new Size(780, 440);
            StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView
            {
                Location = new Point(15, 15),
                Size = new Size(735, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var btnReturn = new Button
            {
                Text = "Return Selected Car",
                Location = new Point(15, 330),
                Size = new Size(170, 40),
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnReturn.Click += BtnReturn_Click;

            var btnClose = new Button
            {
                Text = "Close",
                Location = new Point(660, 330),
                Size = new Size(90, 40),
                BackColor = Color.Gray,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.Click += (s, e) => Close();

            Controls.AddRange(new Control[] { grid, btnReturn, btnClose });
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            grid.DataSource = null;
            grid.DataSource = DataStore.Rentals
                .OrderByDescending(r => r.Id)
                .Select(r => new
                {
                    r.Id,
                    Car = r.CarInfo,
                    Customer = r.CustomerName,
                    RentDate = r.RentDate.ToShortDateString(),
                    ExpectedReturn = r.ExpectedReturnDate.ToShortDateString(),
                    Returned = r.ActualReturnDate.HasValue
                        ? r.ActualReturnDate.Value.ToShortDateString() : "-",
                    Total = "Rs " + r.TotalCost.ToString("N0"),
                    Status = r.IsReturned ? "Returned" : "Active"
                }).ToList();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count == 0)
            { MessageBox.Show("Please select a rental first."); return; }

            int id = (int)grid.SelectedRows[0].Cells["Id"].Value;
            var rental = DataStore.Rentals.FirstOrDefault(r => r.Id == id);
            if (rental == null) return;

            if (rental.IsReturned)
            { MessageBox.Show("This car has already been returned."); return; }

            var car = DataStore.Cars.FirstOrDefault(c => c.Id == rental.CarId);
            if (car == null)
            { MessageBox.Show("Linked car not found."); return; }

            if (MessageBox.Show($"Mark {car.GetDescription()} as returned?",
                    "Confirm Return", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                rental.IsReturned = true;
                rental.ActualReturnDate = DateTime.Today;
                // Recalculate in case it came back late (or early)
                rental.TotalCost = rental.CalculateCost(car.RentPerDay);
                car.IsAvailable = true;
                DataStore.Save();
                RefreshGrid();
                MessageBox.Show($"Car returned.\nFinal bill: Rs {rental.TotalCost:N0} " +
                    $"for {rental.GetRentalDays()} day(s).",
                    "Returned", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
