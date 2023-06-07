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
    /// Interaction logic for AtractionDetails.xaml
    /// </summary>
    public partial class AtractionDetails : UserControl
    {
        private int selectedTripId;
        private int attractionId;
        public TripAttraction SelectedAttraction { get; set; }

        public AtractionDetails(int selectedTripId, int attractionId)
        {
            this.selectedTripId = selectedTripId;
            this.attractionId = attractionId;
            InitializeComponent();
            DataContext = this;
            SelectedAttraction = GetAttraction();
        }

        private TripAttraction GetAttraction()
        {
            var converter = new Base64StringToImageSourceConverter();
            TripAttraction Attraction = new TripAttraction();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Attraction attraction = dbContext.Attractions.Find(attractionId);
                Attraction = new TripAttraction
                        {
                            Location = "Adresa: " + attraction.Location.Address,
                            Id = attraction.Id,
                            TourID = attraction.TourID,
                            Description = attraction.Description,
                            Name = attraction.Name,
                            Image = (BitmapImage)converter.Convert(attraction.Picture, null, null, null)
                        };
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Attraction;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                    if (dbContext.ReservedTours.Find(selectedTripId) != null)
                    {
                        ReservedTourDetails tourDetails = new ReservedTourDetails(selectedTripId);
                        ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                        clientMainWindow.contentControl.Content = tourDetails;
                    }
                    else if (dbContext.BoughtTours.Find(selectedTripId) != null)
                    {
                        BoughtTourDetails tourDetails = new BoughtTourDetails(selectedTripId);
                        ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                        clientMainWindow.contentControl.Content = tourDetails;
                    }
                    else
                    {
                        TourDetails tourDetails = new TourDetails(selectedTripId);
                        ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                        clientMainWindow.contentControl.Content = tourDetails;
                    }
            }
            else {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
