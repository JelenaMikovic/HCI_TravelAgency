using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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
    /// Interaction logic for TourDetails.xaml
    /// </summary>
    public partial class TourDetails : UserControl
    {
        private int selectedTripId;
        private int attractionId;
        public Trip detailedTrip { get; set; }
        public ObservableCollection<TripAttraction> attractions { get; set; }
        public ObservableCollection<TripAccomodation> accomondations { get; set; }
        public ObservableCollection<TripRestaurant> restaurants { get; set; }

        public TourDetails(int selectedTripId)
        {
            this.selectedTripId = selectedTripId;
            InitializeComponent();
            DataContext = this;
            detailedTrip = GetTrip();
            attractions = GetAttractions();
            accomondations = GetAccomondations();
            restaurants = GetRestaurants();
        }

        public Trip GetTrip()
        {
            var converter = new Base64StringToImageSourceConverter();
            Trip found = new Trip();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Tour tour = dbContext.Tours.Find(this.selectedTripId);
                if (tour != null)
                {
                    found = new Trip
                    {
                        Location = tour.Name,
                        DateRange = tour.From.ToString("d") + " - " + tour.To.ToString("d") + " (" + (int)(tour.To - tour.From).TotalDays + " dana)",
                        Price = tour.Price,
                        Id = tour.Id,
                        Image = (BitmapImage)converter.Convert(tour.Picture, null, null, null)
                    };
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return found;
        }

        public ObservableCollection<TripAttraction> GetAttractions()
        {
            var converter = new Base64StringToImageSourceConverter();
            attractions = new ObservableCollection<TripAttraction>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Attraction attraction in dbContext.Attractions)
                {
                    if(attraction.TourID == this.selectedTripId && attraction.isDeleted == false)
                    {
                        attractions.Add(new TripAttraction
                        {
                            Location = attraction.Location.Address,
                            Id = attraction.Id,
                            TourID = attraction.TourID,
                            Description = (attraction.Description.Length > 88 ? attraction.Description.Substring(0, 88) : attraction.Description) + "...",
                            Name = attraction.Name,
                            Image = (BitmapImage)converter.Convert(attraction.Picture, null, null, null)
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return attractions;
        }

        public ObservableCollection<TripRestaurant> GetRestaurants()
        {
            var converter = new Base64StringToImageSourceConverter();
            restaurants = new ObservableCollection<TripRestaurant>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Restaurant restaurant in dbContext.Restaurants)
                {
                    if (restaurant.TourID == this.selectedTripId && restaurant.isDeleted == false)
                    {
                        restaurants.Add(new TripRestaurant
                        {
                            Location = restaurant.Location.Address,
                            Id = restaurant.Id,
                            TourID = restaurant.TourID,
                            Description = (restaurant.Description.Length > 88 ? restaurant.Description.Substring(0, 88) : restaurant.Description) + "...",
                            Name = restaurant.Name,
                            Image = (BitmapImage)converter.Convert(restaurant.Picture, null, null, null)
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return restaurants;
        }

        public ObservableCollection<TripAccomodation> GetAccomondations()
        {
            var converter = new Base64StringToImageSourceConverter();
            accomondations = new ObservableCollection<TripAccomodation>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Accomondation accomondation in dbContext.Accomondations)
                {
                    if (accomondation.TourID == this.selectedTripId && accomondation.isDeleted == false)
                    {
                        accomondations.Add(new TripAccomodation
                        {
                            Location = accomondation.Location.Address,
                            Id = accomondation.Id,
                            TourID = accomondation.TourID,
                            Type = accomondation.Type.ToString(),
                            Name = accomondation.Name,
                            Image = (BitmapImage)converter.Convert(accomondation.Picture, null, null, null)
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return accomondations;
        }

        public void Buy(object sender, RoutedEventArgs e)
        {

        }

        public void Reserve(object sender, RoutedEventArgs e)
        {

        }

        private void Back(object sender, RoutedEventArgs e)
        {
            FutureTrips futureTrips = new FutureTrips();
            ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = futureTrips;
        }

        private void AttractionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (attractionList.SelectedItem != null)
            {
                var attractionId = ((TripAttraction)attractionList.SelectedItem).Id;
                this.attractionId = attractionId;
                AtractionDetails atractionDetails = new AtractionDetails(selectedTripId, attractionId);
                ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = atractionDetails;
            }
        }
    }

    public class TripAttraction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public BitmapImage Image { get; set; }
        public int TourID { get; set; }
    }

    public class TripRestaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Location { get; set; }
        public BitmapImage Image { get; set; }
        public int TourID { get; set; }

    }

    public class TripAccomodation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public int TourID { get; set; }
        public BitmapImage Image { get; set; }
    }

}
