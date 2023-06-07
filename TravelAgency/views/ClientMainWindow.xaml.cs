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
using TravelAgency.views;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for ClientMainWindow.xaml
    /// </summary>
    public partial class ClientMainWindow : Window
    {
        public ClientMainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            string tag = clickedButton.Tag.ToString();

            // Create the corresponding user control based on the tag value
            UserControl content = null;
            switch (tag)
            {
                case "Home":
                    content = new FutureTrips();
                    break;
                case "Purchased":
                    content = new TourDetails(1);
                    break;
                case "Reserved":
                    content = new ReservedTours();
                    break;
                    // Add more cases for additional buttons if needed
            }

            // Set the content of the ContentControl to the selected user control
            contentControl.Content = content;

        }

        private void LogOut(object sender, RoutedEventArgs e)
        {

        }

    }
}
