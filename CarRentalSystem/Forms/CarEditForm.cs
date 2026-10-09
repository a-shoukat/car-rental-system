using System;
using System.Drawing;
using System.Windows.Forms;
using CarRentalSystem.Data;
using CarRentalSystem.Models;

namespace CarRentalSystem.Forms
{
    // Dialog used for both adding a new car and editing an existing one
    public class CarEditForm : Form
    {
        private TextBox txtMake, txtModel, txtYear, txtColor, txtPlate, txtRent;
        public Car ResultCar { get; private set; }
        private readonly Car editing;

        public CarEditForm() : this(null) { }

        public CarEditForm(Car carToEdit)
        {
            editing = carToEdit;
            Text = carToEdit == null ? "Add New Car" : "Edit Car";
            Size = new Size(380, 380);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            string[] labels = { "Make:", "Model:", "Year:", "Color:", "License Plate:", "Rent / Day (Rs):" };
            TextBox[] boxes = new TextBox[labels.Length];

            for (int i = 0; i < labels.Length; i++)
            {
                var lbl = new Label { Text = labels[i], Location = new Point(25, 25 + i * 45), AutoSize = true };
                var txt = new TextBox { Location = new Point(150, 22 + i * 45), Width = 180 };
                boxes[i] = txt;
                Controls.Add(lbl);
                Controls.Add(txt);
            }
            txtMake = boxes[0]; txtModel = boxes[1]; txtYear = boxes[2];
            txtColor = boxes[3]; txtPlate = boxes[4]; txtRent = boxes[5];

            if (editing != null)
            {
                txtMake.Text = editing.Make;
                txtModel.Text = editing.Model;
                txtYear.Text = editing.Year.ToString();
                txtColor.Text = editing.Color;
                txtPlate.Text = editing.LicensePlate;
                txtRent.Text = editing.RentPerDay.ToString();
            }

            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(150, 300),
                Width = 180,
                BackColor = Color.FromArgb(15, 118, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.Click += BtnSave_Click;
            Controls.Add(btnSave);
            AcceptButton = btnSave;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMake.Text) ||
                string.IsNullOrWhiteSpace(txtModel.Text) ||
                string.IsNullOrWhiteSpace(txtPlate.Text))
            {
                MessageBox.Show("Make, Model and License Plate are required.");
                return;
            }
            if (!int.TryParse(txtYear.Text, out int year) || year < 1990 || year > 2030)
            {
                MessageBox.Show("Please enter a valid year (1990-2030).");
                return;
            }
            if (!decimal.TryParse(txtRent.Text, out decimal rent) || rent <= 0)
            {
                MessageBox.Show("Please enter a valid rent per day.");
                return;
            }

            if (editing == null)
            {
                ResultCar = new Car(DataStore.NextCarId(), txtMake.Text.Trim(),
                    txtModel.Text.Trim(), year, txtColor.Text.Trim(),
                    txtPlate.Text.Trim(), rent);
            }
            else
            {
                editing.Make = txtMake.Text.Trim();
                editing.Model = txtModel.Text.Trim();
                editing.Year = year;
                editing.Color = txtColor.Text.Trim();
                editing.LicensePlate = txtPlate.Text.Trim();
                editing.RentPerDay = rent;
                ResultCar = editing;
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
