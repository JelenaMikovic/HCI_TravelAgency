using MahApps.Metro.Controls;
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
    /// Interaction logic for AgentMainWindow.xaml
    /// </summary>
    public partial class AgentMainWindow : Window
    {
        public AgentMainWindow()
        {
            InitializeComponent();
            UserControl content = new AgentFutureTrips();
            contentControl.Content = content;
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
                    content = new AgentFutureTrips();
                    break;
                case "CreateNew":
                    content = new CreateNewTour();
                    break;
                case "TripsByMonth":
                    content = new TripsByMonth();
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
