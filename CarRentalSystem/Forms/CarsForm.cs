using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarRentalSystem.Data;
using CarRentalSystem.Models;

namespace CarRentalSystem.Forms
{
    public class CarsForm : Form
    {
        private DataGridView grid;

        public CarsForm()
        {
            Text = "Manage Cars";
            Size = new Size(760, 440);
            StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView
            {
                Location = new Point(15, 15),
                Size = new Size(715, 300),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var btnAdd = MakeButton("Add Car", 15, BtnAdd_Click);
            var btnEdit = MakeButton("Edit Selected", 135, BtnEdit_Click);
            var btnDelete = MakeButton("Delete Selected", 255, BtnDelete_Click);
            var btnClose = MakeButton("Close", 615, (s, e) => Close());
            btnClose.BackColor = Color.Gray;

            Controls.AddRange(new Control[] { grid, btnAdd, btnEdit, btnDelete, btnClose });
            RefreshGrid();
        }

        private Button MakeButton(string text, int x, EventHandler onClick)
        {
            var b = new Button
            {
                Text = text,
                Location = new Point(x, 330),
                Size = new Size(110, 40),
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            b.Click += onClick;
            return b;
        }

        private void RefreshGrid()
        {
            grid.DataSource = null;
            grid.DataSource = DataStore.Cars.Select(c => new
            {
                c.Id,
                c.Make,
                c.Model,
                c.Year,
                c.Color,
                Plate = c.LicensePlate,
                RentPerDay = "Rs " + c.RentPerDay,
                Status = c.IsAvailable ? "Available" : "Rented"
            }).ToList();
        }

        private Car SelectedCar()
        {
            if (grid.SelectedRows.Count == 0) return null;
            int id = (int)grid.SelectedRows[0].Cells["Id"].Value;
            return DataStore.Cars.FirstOrDefault(c => c.Id == id);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var f = new CarEditForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    DataStore.Cars.Add(f.ResultCar);
                    DataStore.Save();
                    RefreshGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var car = SelectedCar();
            if (car == null) { MessageBox.Show("Please select a car first."); return; }
            using (var f = new CarEditForm(car))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    DataStore.Save();
                    RefreshGrid();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var car = SelectedCar();
            if (car == null) { MessageBox.Show("Please select a car first."); return; }
            if (!car.IsAvailable)
            {
                MessageBox.Show("This car is currently rented and cannot be deleted.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Delete {car.GetDescription()}?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataStore.Cars.Remove(car);
                DataStore.Save();
                RefreshGrid();
            }
        }
    }
}
