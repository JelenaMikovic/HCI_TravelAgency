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
    /// Interaction logic for TourDetailsAgent.xaml
    /// </summary>
    public partial class TourDetailsAgent : UserControl
    {
        private int selectedTripId;
        private int attractionId;
        private int restaurantId;
        public Trip detailedTrip { get; set; }
        public ObservableCollection<TripAttraction> attractions { get; set; }
        public ObservableCollection<TripAccomodation> accomondations { get; set; }
        public ObservableCollection<TripRestaurant> restaurants { get; set; }
        public TourDetailsAgent(int selectedTripId)
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
                    if (attraction.TourID == this.selectedTripId && attraction.isDeleted == false)
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

        public void Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Molimo Vas da potvrdite brisanje.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Application.Current.Resources["DbContext"] is DbContext dbContext)
                {
                    foreach (TripRestaurant res in restaurants)
                    {
                        Restaurant attraction = dbContext.Restaurants.Find(res.Id);
                        Location location = dbContext.Locations.Find(attraction.Location.Id);
                        dbContext.Locations.Remove(location);
                        dbContext.Restaurants.Remove(attraction);
                    }
                    foreach (TripAttraction res in attractions)
                    {
                        Attraction attraction = dbContext.Attractions.Find(res.Id);
                        Location location = dbContext.Locations.Find(attraction.Location.Id);
                        dbContext.Locations.Remove(location);
                        dbContext.Attractions.Remove(attraction);
                    }
                    foreach (TripAccomodation res in accomondations)
                    {
                        Accomondation attraction = dbContext.Accomondations.Find(res.Id);
                        Location location = dbContext.Locations.Find(attraction.Location.Id);
                        dbContext.Locations.Remove(location);
                        dbContext.Accomondations.Remove(attraction);
                    }
                    foreach (BoughtTour res in dbContext.BoughtTours)
                    {
                        if(res.TourId == selectedTripId)
                        {
                            dbContext.BoughtTours.Remove(res);
                        }
                     
                    }
                    foreach (ReservedTour res in dbContext.ReservedTours)
                    {
                        if (res.TourId == selectedTripId)
                        {
                            dbContext.ReservedTours.Remove(res);
                        }

                    }
                    Tour t = dbContext.Tours.Find(selectedTripId);
                    Location loc = dbContext.Locations.Find(t.StartingLocation.Id);
                    dbContext.Locations.Remove(loc);
                    dbContext.Tours.Remove(t);
                    AgentFutureTrips tourDetails = new AgentFutureTrips();
                    AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                    clientMainWindow.contentControl.Content = tourDetails;
                }
                else
                {
                    MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }

        private void Back(object sender, RoutedEventArgs e)
        {
            AgentFutureTrips futureTrips = new AgentFutureTrips();
            AgentMainWindow agentMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            agentMainWindow.contentControl.Content = futureTrips;
        }

        private void AttractionSelected(object sender, SelectionChangedEventArgs e)
        {
            if (attractionList.SelectedItem != null)
            {
                var attractionId = ((TripAttraction)attractionList.SelectedItem).Id;
                this.attractionId = attractionId;
                AtractionDetails atractionDetails = new AtractionDetails(selectedTripId, attractionId);
                AgentMainWindow agentMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                agentMainWindow.contentControl.Content = atractionDetails;
            }
        }

        private void RestaurantSelected(object sender, SelectionChangedEventArgs e)
        {
            if (restaurantList.SelectedItem != null)
            {
                var restaurantId = ((TripRestaurant)restaurantList.SelectedItem).Id;
                this.restaurantId = restaurantId;
                RestaurantDetails atractionDetails = new RestaurantDetails(selectedTripId, restaurantId);
                AgentMainWindow agentMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                agentMainWindow.contentControl.Content = atractionDetails;
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            EditTourMain editTour = new EditTourMain(this.selectedTripId);
            AgentMainWindow agentMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            agentMainWindow.contentControl.Content = editTour;
        }
    }
}
