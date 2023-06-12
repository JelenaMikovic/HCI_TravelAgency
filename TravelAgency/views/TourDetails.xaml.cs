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
using Microsoft.Maps.MapControl.WPF;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Net.Http;
using System.Web;
using Location = Microsoft.Maps.MapControl.WPF.Location;
using Nest;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Controls.Primitives;

namespace TravelAgency.views
{
    /// <summary>
    /// Interaction logic for TourDetails.xaml
    /// </summary>
    public partial class TourDetails : UserControl
    {
        private int selectedTripId;
        private int attractionId;
        private int restaurantId;
        public Trip detailedTrip { get; set; }
        public ObservableCollection<TripAttraction> attractions { get; set; }
        public ObservableCollection<TripAccomodation> accomondations { get; set; }
        public ObservableCollection<TripRestaurant> restaurants { get; set; }

        private const string apiKey = "yQPugx2BdP4Z4jFOgqA6~vbvF_IqjsRBEaAvEzIZjng~Aji6WgIN35bikdwGC0vNFmFXZNmHbM0kHk4SSM_l2xHldQ2cSukhAB-3oYQujGjS";


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
                        GeocodeAddress(attraction.Location, attraction.Name);
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
                        GeocodeAddress(restaurant.Location, restaurant.Name);
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
                        GeocodeAddress(accomondation.Location, accomondation.Name);
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
            BuyTrip futureTrips = new BuyTrip(selectedTripId);
            ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = futureTrips;
        }

        public void Reserve(object sender, RoutedEventArgs e)
        {
            ReserveTrip futureTrips = new ReserveTrip(selectedTripId);
            ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = futureTrips;

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

        private void RestaurantSelected(object sender, SelectionChangedEventArgs e)
        {
            if (restaurantList.SelectedItem != null)
            {
                var restaurantId = ((TripRestaurant)restaurantList.SelectedItem).Id;
                this.restaurantId = restaurantId;
                RestaurantDetails atractionDetails = new RestaurantDetails(selectedTripId, restaurantId);
                ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = atractionDetails;
            }
        }

        private void ShowMap(object sender, RoutedEventArgs e)
        {
            mapPopup.IsOpen = true;
        }
        private void CloseMap(object sender, RoutedEventArgs e)
        {
            mapPopup.IsOpen = false;
        }

        private void Popup_Loaded(object sender, RoutedEventArgs e)
        {
            var popup = (Popup)sender;
            var transform = (TranslateTransform)popup.RenderTransform;

            var ownerWindow = Window.GetWindow(this);
            if (ownerWindow != null)
            {
                var offset = (ownerWindow.ActualWidth - popup.ActualWidth) / 2;
                transform.X = offset;
            }
        }

        private async Task GeocodeAddress(model.Location location, string name)
        {
            string apiUrl = "https://dev.virtualearth.net/REST/v1/Locations";
            string encodedCountry = HttpUtility.UrlEncode(location.Country);
            string encodedCity = HttpUtility.UrlEncode(location.City);
            string encodedAddress = HttpUtility.UrlEncode(location.Address);
            string requestUrl = $"{apiUrl}?countryRegion={encodedCountry}&locality={encodedCity}&addressLine={encodedAddress}&key={apiKey}";


            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(jsonResponse);
                    var resourceSets = data.resourceSets;
                    if (resourceSets.Count > 0)
                    {
                        var resources = resourceSets[0].resources;
                        if (resources.Count > 0)
                        {
                            var point = resources[0].point;
                            double latitude = point.coordinates[0];
                            double longitude = point.coordinates[1];

                            myMap.Center = new Location(latitude, longitude);
                            myMap.ZoomLevel = 15;

                            // Create a grid container to hold the pin and name
                            var grid = new Grid();

                            // Add a pin to the grid
                            var pin = new Pushpin();
                            Grid.SetRow(pin, 0); 
                            pin.FontSize = 12; // Adjust the pin font size as needed
                            grid.Children.Add(pin);

                            // Add the name to the grid
                            var nameTextBlock = new TextBlock();
                            Grid.SetRow(nameTextBlock, 1);
                            nameTextBlock.Text = name;
                            nameTextBlock.FontSize = 12; // Adjust the name font size as needed
                            nameTextBlock.FontWeight = FontWeights.Bold;
                            nameTextBlock.Foreground = new SolidColorBrush(Colors.Black);
                            grid.Children.Add(nameTextBlock);

                            // Add the grid to the map
                            MapLayer.SetPosition(grid, new Location(latitude, longitude));
                            myMap.Children.Add(grid);
                        }
                    }
                }
            }
        }

        private void ShowHelp_Click(object sender, RoutedEventArgs e)
        {
            Help display = new Help("/../../../help/ClientFutureTourDetails.html");
            display.ShowDialog();
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
