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
    /// Interaction logic for BuyTrip.xaml
    /// </summary>
    public partial class BuyTrip : UserControl
    {
        private int selectedTripId;
        public Trip detailedTrip { get; set; }
        public ObservableCollection<TripAttraction> attractions { get; set; }
        public ObservableCollection<TripAccomodation> accomondations { get; set; }
        public ObservableCollection<TripRestaurant> restaurants { get; set; }
        public List<TripAttraction> selctedAttractions { get; set; }
        public List<TripRestaurant> selectedRestaurants { get; set; }
        public List<TripAccomodation> selectedAccomondation { get; set; }
        public BuyTrip(int selectedTripId)
        {
            InitializeComponent();
            this.selectedTripId = selectedTripId;
            DataContext = this;
            detailedTrip = GetTrip();
            attractions = GetAttractions();
            accomondations = GetAccomondations();
            restaurants = GetRestaurants();
            selctedAttractions = new List<TripAttraction>();
            selectedRestaurants = new List<TripRestaurant>();
            selectedAccomondation = new List<TripAccomodation>();
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

        private int currentStep = 1;

        public int SelectedTripId { get; }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep == 1)
            {
                selctedAttractions.Clear();
                foreach (var selectedItem in attractionList.SelectedItems)
                {
                    selctedAttractions.Add((TripAttraction)selectedItem);
                }
            }
            else if (currentStep == 2)
            {
                selectedRestaurants.Clear();
                foreach (var selectedItem in restaurantList.SelectedItems)
                {
                    selectedRestaurants.Add((TripRestaurant)selectedItem);
                }
            }
            else if (currentStep == 3)
            {
                if (accomondationList.SelectedItem != null)
                {
                    selectedAccomondation.Clear();
                    selectedAccomondation.Add((TripAccomodation)accomondationList.SelectedItem);
                    FinilizeBuy reservation = new FinilizeBuy(selctedAttractions, selectedAccomondation, selectedRestaurants, detailedTrip);
                    ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                    clientMainWindow.contentControl.Content = reservation;
                }
                else
                {
                    MessageBox.Show("Morate da izaberete smestaj!", "Nepravilno odabrano", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }
            if (currentStep < 3)
            {
                currentStep++;
                UpdateStepContentVisibility();
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                UpdateStepContentVisibility();
            }
        }

        private void UpdateStepContentVisibility()
        {
            Step1Content.Visibility = currentStep == 1 ? Visibility.Visible : Visibility.Collapsed;
            Step2Content.Visibility = currentStep == 2 ? Visibility.Visible : Visibility.Collapsed;
            Step3Content.Visibility = currentStep == 3 ? Visibility.Visible : Visibility.Collapsed;

            PreviousButton.Visibility = currentStep == 1 ? Visibility.Collapsed : Visibility.Visible;
            NextButton.Content = currentStep == 3 ? "Kupi" : "Dalje";
        }
    }
}
