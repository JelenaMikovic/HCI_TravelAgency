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
using TravelAgency.converters;
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
       
            DataContext = this;

            Trips = this.GetTrips();
        }

        private ObservableCollection<Trip>? GetTrips()
        {
            var converter = new Base64StringToImageSourceConverter();
            Trips = new ObservableCollection<Trip>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Tour tour in dbContext.Tours)
                {
                    Trips.Add(new Trip { Location = tour.Name, DateRange = tour.From.ToString("d") + " - " + tour.To.ToString("d") + " (" + (int)(tour.To - tour.From).TotalDays + " dana)", Price = tour.Price,
                        Id = tour.Id,
                        Image = (BitmapImage)converter.Convert(tour.Picture, null, null, null)
                    });
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Trips;
        }

        private int SelectedTripId { get; set; }

        private void TripListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tripListBox.SelectedItem != null)
            {
                var selectedTripId = ((Trip)tripListBox.SelectedItem).Id;
                SelectedTripId = selectedTripId;
                TourDetails tourDetails = new TourDetails(selectedTripId);
                ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = tourDetails;
            }
        }

        private void ShowHelp_Click(object sender, RoutedEventArgs e)
        {
            Help display = new Help("/../../../help/ClientFutureTours.html");
            display.ShowDialog();
        }
    }

    public class Trip
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string DateRange { get; set; }
        public int Price { get; set; }
        public BitmapImage Image { get; set; }
    }
}
