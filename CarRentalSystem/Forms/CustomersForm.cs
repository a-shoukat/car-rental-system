using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CarRentalSystem.Data;
using CarRentalSystem.Models;

namespace CarRentalSystem.Forms
{
    public class CustomersForm : Form
    {
        private DataGridView grid;

        public CustomersForm()
        {
            Text = "Manage Customers";
            Size = new Size(680, 420);
            StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView
            {
                Location = new Point(15, 15),
                Size = new Size(635, 290),
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            var btnAdd = MakeButton("Add Customer", 15, BtnAdd_Click);
            var btnEdit = MakeButton("Edit Selected", 145, BtnEdit_Click);
            var btnDelete = MakeButton("Delete Selected", 275, BtnDelete_Click);
            var btnClose = MakeButton("Close", 540, (s, e) => Close());
            btnClose.BackColor = Color.Gray;

            Controls.AddRange(new Control[] { grid, btnAdd, btnEdit, btnDelete, btnClose });
            RefreshGrid();
        }

        private Button MakeButton(string text, int x, EventHandler onClick)
        {
            var b = new Button
            {
                Text = text,
                Location = new Point(x, 320),
                Size = new Size(120, 40),
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
            grid.DataSource = DataStore.Customers.Select(c => new
            {
                c.Id,
                c.Name,
                c.Phone,
                License = c.LicenseNumber
            }).ToList();
        }

        private Customer SelectedCustomer()
        {
            if (grid.SelectedRows.Count == 0) return null;
            int id = (int)grid.SelectedRows[0].Cells["Id"].Value;
            return DataStore.Customers.FirstOrDefault(c => c.Id == id);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            using (var f = new CustomerEditForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    DataStore.Customers.Add(f.ResultCustomer);
                    DataStore.Save();
                    RefreshGrid();
                }
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            var customer = SelectedCustomer();
            if (customer == null) { MessageBox.Show("Please select a customer first."); return; }
            using (var f = new CustomerEditForm(customer))
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
            var customer = SelectedCustomer();
            if (customer == null) { MessageBox.Show("Please select a customer first."); return; }
            bool hasActive = DataStore.Rentals.Any(r => r.CustomerId == customer.Id && !r.IsReturned);
            if (hasActive)
            {
                MessageBox.Show("This customer has an active rental and cannot be deleted.",
                    "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show($"Delete customer {customer.Name}?", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataStore.Customers.Remove(customer);
                DataStore.Save();
                RefreshGrid();
            }
        }
    }

    public class CustomerEditForm : Form
    {
        private TextBox txtName, txtPhone, txtLicense;
        public Customer ResultCustomer { get; private set; }
        private readonly Customer editing;

        public CustomerEditForm() : this(null) { }

        public CustomerEditForm(Customer customerToEdit)
        {
            editing = customerToEdit;
            Text = customerToEdit == null ? "Add New Customer" : "Edit Customer";
            Size = new Size(380, 260);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            string[] labels = { "Full Name:", "Phone:", "Driving License No:" };
            TextBox[] boxes = new TextBox[labels.Length];
            for (int i = 0; i < labels.Length; i++)
            {
                Controls.Add(new Label { Text = labels[i], Location = new Point(25, 25 + i * 45), AutoSize = true });
                boxes[i] = new TextBox { Location = new Point(160, 22 + i * 45), Width = 170 };
                Controls.Add(boxes[i]);
            }
            txtName = boxes[0]; txtPhone = boxes[1]; txtLicense = boxes[2];

            if (editing != null)
            {
                txtName.Text = editing.Name;
                txtPhone.Text = editing.Phone;
                txtLicense.Text = editing.LicenseNumber;
            }

            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(160, 170),
                Width = 170,
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
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Name and Phone are required.");
                return;
            }
            if (editing == null)
            {
                ResultCustomer = new Customer(DataStore.NextCustomerId(),
                    txtName.Text.Trim(), txtPhone.Text.Trim(), txtLicense.Text.Trim());
            }
            else
            {
                editing.Name = txtName.Text.Trim();
                editing.Phone = txtPhone.Text.Trim();
                editing.LicenseNumber = txtLicense.Text.Trim();
                ResultCustomer = editing;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
