using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using TravelAgency.views;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
      
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Register(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                if (dbContext.Users.Any(u => u.Email == txtEmail.Text))
                {
                    MessageBox.Show("Ovaj mail je vec u upotrebi, pokusajte neki drugi!");
                    return;
                }

                User newUser = new User
                {
                    Name = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    PhoneNumber = txtPhoneNumber.Text,
                    Password = txtPassword.Password,
                    UserRole = UserRole.CLIENT
                };

                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                MessageBox.Show("Registracija uspesna!");
                LogInWindow mainWindow = new LogInWindow();
                Application.Current.MainWindow = mainWindow;
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            ClearForm();
        }

        private bool ValidateInputs()
        {
            NotEmptyValidationRule notEmptyRule = new NotEmptyValidationRule();
            EmailValidationRule emailRule = new EmailValidationRule();
            PhoneNumberValidationRule phoneNumberRule = new PhoneNumberValidationRule();
            CharacterLimitValidationRule characterLimitRule = new CharacterLimitValidationRule();

            notEmptyRule.ErrorMessage = "Ime je obavezno polje.";
            ValidationResult firstNameValidationResult = notEmptyRule.Validate(txtFirstName.Text, CultureInfo.CurrentCulture);
            if (!firstNameValidationResult.IsValid)
            {
                MessageBox.Show(firstNameValidationResult.ErrorContent.ToString());
                return false;
            }

            notEmptyRule.ErrorMessage = "Prezime je obavezno polje.";
            ValidationResult lastNameValidationResult = notEmptyRule.Validate(txtLastName.Text, CultureInfo.CurrentCulture);
            if (!lastNameValidationResult.IsValid)
            {
                MessageBox.Show(lastNameValidationResult.ErrorContent.ToString());
                return false;
            }

            emailRule.ErrorMessage = "Mail nije validan.";
            ValidationResult emailValidationResult = emailRule.Validate(txtEmail.Text, CultureInfo.CurrentCulture);
            if (!emailValidationResult.IsValid)
            {
                MessageBox.Show(emailValidationResult.ErrorContent.ToString());
                return false;
            }

            phoneNumberRule.ErrorMessage = "Telefonski broj nije validan.";
            ValidationResult phoneNumberValidationResult = phoneNumberRule.Validate(txtPhoneNumber.Text, CultureInfo.CurrentCulture);
            if (!phoneNumberValidationResult.IsValid)
            {
                MessageBox.Show(phoneNumberValidationResult.ErrorContent.ToString());
                return false;
            }

            characterLimitRule.ErrorMessage = "Lozinka mora da ima izmedju 8 i 128 karaktera.";
            ValidationResult passwordValidationResult = characterLimitRule.Validate(txtPassword.Password, CultureInfo.CurrentCulture);
            if (!passwordValidationResult.IsValid)
            {
                MessageBox.Show(passwordValidationResult.ErrorContent.ToString());
                return false;
            }

            return true;
        }

        private void ClearForm()
        {

            txtFirstName.Focus();
        }

        private void LogIn(object sender, RoutedEventArgs e)
        {
            LogInWindow mainWindow = new LogInWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }
    }

    public class NotEmptyValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return new ValidationResult(false, ErrorMessage);

            return ValidationResult.ValidResult;
        }
    }

    public class EmailValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string email = value?.ToString();
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, ErrorMessage);

            Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            if (!regex.IsMatch(email))
                return new ValidationResult(false, ErrorMessage);

            return ValidationResult.ValidResult;
        }
    }

    public class PhoneNumberValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string phoneNumber = value?.ToString();
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return new ValidationResult(false, ErrorMessage);

            Regex regex = new Regex(@"^\d{10}$");
            if (!regex.IsMatch(phoneNumber))
                return new ValidationResult(false, ErrorMessage);

            return ValidationResult.ValidResult;
        }
    }

    public class CharacterLimitValidationRule : ValidationRule
    {
        public string ErrorMessage { get; set; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string text = value?.ToString();
            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, ErrorMessage);

            if (text.Length < 8 || text.Length > 128)
                return new ValidationResult(false, ErrorMessage);

            return ValidationResult.ValidResult;
        }
    }
}