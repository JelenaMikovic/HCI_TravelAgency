using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelAgency.db;
using TravelAgency.model;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            RegistrationWindow mainWindow = new RegistrationWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }

            private void LogIn(object sender, RoutedEventArgs e)
        {

            string email = emailTextBox.Text.Trim();
            string password = passwordBox.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter a valid email and password.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                AgentMainWindow mainWindow = new AgentMainWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                Close();
                return;
            }

            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                User user = dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

                if (user != null)
                {
                    LoggedInUser.CurrentUser = user;
                    if (user.UserRole.Equals(UserRole.AGENT)) {
                        AgentMainWindow mainWindow = new AgentMainWindow();
                        Application.Current.MainWindow = mainWindow;
                        mainWindow.Show();
                        Close();
                    }
                    else {
                        ClientMainWindow mainWindow = new ClientMainWindow();
                        Application.Current.MainWindow = mainWindow;
                        mainWindow.Show();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
