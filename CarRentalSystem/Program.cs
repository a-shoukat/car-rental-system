using System;
using System.Windows.Forms;
using CarRentalSystem.Data;
using CarRentalSystem.Forms;

namespace CarRentalSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DataStore.Load();

            using (var login = new LoginForm())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new MainForm());
                }
            }
        }
    }
}
