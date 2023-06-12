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
    /// Interaction logic for Arrangements.xaml
    /// </summary>
    public partial class Arrangements : UserControl
    {
        public ObservableCollection<TripArrangement> Trips { get; set; }

        public Arrangements()
        {
            InitializeComponent();

            DataContext = this;

            Trips = this.GetTrips();
        }

        private ObservableCollection<TripArrangement>? GetTrips()
        {
            var converter = new Base64StringToImageSourceConverter();
            Trips = new ObservableCollection<TripArrangement>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Tour tour in dbContext.Tours)
                {
                    int Bought = 0;
                    int Reserved = 0;
                    foreach (BoughtTour bought in dbContext.BoughtTours)
                    {
                        if(bought.TourId == tour.Id) { Bought++; }
                    }
                    foreach (ReservedTour reserved in dbContext.ReservedTours)
                    {
                        if (reserved.TourId == tour.Id) { Reserved++; }
                    }
                    Trips.Add(new TripArrangement
                    {
                        Location = tour.Name,
                        DateRange = tour.From.ToString("d") + " - " + tour.To.ToString("d") + " (" + (int)(tour.To - tour.From).TotalDays + " dana)",
                        Price = tour.Price,
                        Id = tour.Id,
                        Image = (BitmapImage)converter.Convert(tour.Picture, null, null, null),
                        Bought = Bought,
                        Reserved = Reserved
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
                TourDetailsAgent tourDetails = new TourDetailsAgent(selectedTripId);
                AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = tourDetails;
            }
        }


    }

    public class TripArrangement
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string DateRange { get; set; }
        public int Price { get; set; }
        public BitmapImage Image { get; set; }
        public int Bought { get; set; }
        public int Reserved { get; set; }
    }
}