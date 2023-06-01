using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.db;
using TravelAgency.model;

namespace TravelAgency.views
{
    /// <summary>
    /// Interaction logic for FutureTrips.xaml
    /// </summary>
    public partial class FutureTrips : UserControl
    {
        public ObservableCollection<Trip> Trips { get; set; }
        public FutureTrips()
        {
            InitializeComponent();
       
            InitializeComponent();
            DataContext = this;

            Trips = this.GetTrips();
        }

        private ObservableCollection<Trip>? GetTrips()
        {
            Trips = new ObservableCollection<Trip>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Tour tour in dbContext.Tours)
                {
                    Trips.Add(new Trip { Location = tour.Name, DateRange = tour.From.ToString("d") + " - " + tour.To.ToString("d") + " (" + (int)(tour.To - tour.From).TotalDays + " dana)", Price = tour.Price, Id = tour.Id});
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Trips;
        }

        private void TripListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Retrieve the selected ListBoxItem
            var listBox = (ListBox)sender;
            var selectedListBoxItem = (ListBoxItem)listBox.SelectedItem;

            // Retrieve the trip Id from the ListBoxItem's Tag property
            var selectedTripId = (int)selectedListBoxItem.Tag;

            // Open a new window or page and pass the selected trip Id as a parameter
            // For example:
            // var tripDetailsWindow = new TripDetailsWindow(selectedTripId);
            // tripDetailsWindow.Show();
            var tripDetails = new TourDetails();
            tripDetails.DataContext = selectedTripId;
        }

    }

    public class Trip
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string DateRange { get; set; }
        public int Price { get; set; }
        public Image Image { get; set; }
    }
}
