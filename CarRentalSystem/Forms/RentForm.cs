using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarRentalSystem.Data;
using CarRentalSystem.Models;

namespace CarRentalSystem.Forms
{
    public class RentForm : Form
    {
        private ComboBox cmbCars, cmbCustomers;
        private DateTimePicker dtpRent, dtpReturn;
        private Label lblCost;

        public RentForm()
        {
            Text = "Rent a Car";
            Size = new Size(440, 360);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var availableCars = DataStore.Cars.Where(c => c.IsAvailable).ToList();
            if (availableCars.Count == 0)
            {
                MessageBox.Show("No cars are currently available for rent.",
                    "No Cars", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if (DataStore.Customers.Count == 0)
            {
                MessageBox.Show("Please add a customer first (Manage Customers).",
                    "No Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            AddLabel("Select Car:", 25);
            cmbCars = new ComboBox
            {
                Location = new Point(170, 22), Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var car in availableCars) cmbCars.Items.Add(car);
            if (cmbCars.Items.Count > 0) cmbCars.SelectedIndex = 0;

            AddLabel("Select Customer:", 70);
            cmbCustomers = new ComboBox
            {
                Location = new Point(170, 67), Width = 220,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var cust in DataStore.Customers) cmbCustomers.Items.Add(cust);
            if (cmbCustomers.Items.Count > 0) cmbCustomers.SelectedIndex = 0;

            AddLabel("Rent Date:", 115);
            dtpRent = new DateTimePicker { Location = new Point(170, 112), Width = 220, Value = DateTime.Today };

            AddLabel("Return Date:", 160);
            dtpReturn = new DateTimePicker { Location = new Point(170, 157), Width = 220, Value = DateTime.Today.AddDays(3) };
            dtpReturn.ValueChanged += (s, e) => UpdateCost();
            cmbCars.SelectedIndexChanged += (s, e) => UpdateCost();

            lblCost = new Label
            {
                Text = "Total Cost: Rs 0",
                Location = new Point(25, 205),
                AutoSize = true,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(13, 42, 67)
            };

            var btnConfirm = new Button
            {
                Text = "Confirm Rental",
                Location = new Point(170, 250),
                Width = 220,
                Height = 40,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnConfirm.Click += BtnConfirm_Click;

            Controls.AddRange(new Control[] { cmbCars, cmbCustomers, dtpRent, dtpReturn, lblCost, btnConfirm });
            UpdateCost();
        }

        private void AddLabel(string text, int y)
        {
            Controls.Add(new Label { Text = text, Location = new Point(25, y + 3), AutoSize = true });
        }

        private void UpdateCost()
        {
            if (cmbCars.SelectedItem is Car car)
            {
                int days = (dtpReturn.Value.Date - dtpRent.Value.Date).Days;
                if (days < 1) days = 1;
                lblCost.Text = $"Total Cost: Rs {days * car.RentPerDay:N0}  ({days} day(s))";
            }
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (!(cmbCars.SelectedItem is Car car))
            { MessageBox.Show("No available car selected."); return; }
            if (!(cmbCustomers.SelectedItem is Customer customer))
            { MessageBox.Show("Please select a customer."); return; }
            if (dtpReturn.Value.Date < dtpRent.Value.Date)
            { MessageBox.Show("Return date cannot be before rent date."); return; }

            var rental = new Rental(DataStore.NextRentalId(), car.Id, customer.Id,
                dtpRent.Value.Date, dtpReturn.Value.Date);
            rental.TotalCost = rental.CalculateCost(car.RentPerDay);
            rental.CarInfo = car.GetDescription();
            rental.CustomerName = customer.Name;

            car.IsAvailable = false;
            DataStore.Rentals.Add(rental);
            DataStore.Save();

            MessageBox.Show(
                $"Rental confirmed!\n\nCar: {car.GetDescription()}\nCustomer: {customer.Name}\n" +
                $"Days: {rental.GetRentalDays()}\nTotal: Rs {rental.TotalCost:N0}",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
