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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.db;
using TravelAgency.model;

namespace TravelAgency.views
{
    /// <summary>
    /// Interaction logic for FinilzeReservation.xaml
    /// </summary>
    public partial class FinilzeReservation : UserControl
    {

        public List<TripAttraction> attractions { get; set; }
        public List<TripAccomodation> accomondations { get; set; }
        public List<TripRestaurant> restaurants { get; set; }
        public Trip detailedTrip { get; set; }

        public FinilzeReservation(List<TripAttraction> selctedAttractions, List<TripAccomodation> selectedAccomondation, List<TripRestaurant> selectedRestaurants, Trip detailedTrip)
        {
            InitializeComponent();
            attractions = selctedAttractions;
            accomondations = selectedAccomondation;
            restaurants = selectedRestaurants;
            this.detailedTrip = detailedTrip;
            DataContext = this;
        }
        private void GiveUp(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da odustanete?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TourDetails tourDetails = new TourDetails(detailedTrip.Id);
                ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = tourDetails;
            }
        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Molimo Vas da potvrdite rezervaciju.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {

                MessageBox.Show("Uspesno ste obavili rezervaciju!", "Obavljena rezervaija", MessageBoxButton.OK, MessageBoxImage.Information);
                if (Application.Current.Resources["DbContext"] is DbContext dbContext)
                {
                    List<Attraction> atr = new List<Attraction>();
                    foreach (TripAttraction i in attractions)
                    {
                        atr.Add(dbContext.Attractions.Find(i.Id));
                    }
                    List<Restaurant> res = new List<Restaurant>();
                    foreach (TripRestaurant i in restaurants)
                    {
                        res.Add(dbContext.Restaurants.Find(i.Id));
                    }
                    Accomondation acc = dbContext.Accomondations.Find(accomondations[0].Id);
                    dbContext.ReservedTours.Add(new ReservedTour
                    {
                        Id = dbContext.ReservedTours.Count() + 100,
                        TourId = detailedTrip.Id,
                        UserId = LoggedInUser.CurrentUser.Id,
                        Attractions = atr,
                        Restaurants = res,
                        Accomondation = acc,
                        isDeleted = false
                    });

                    dbContext.SaveChanges();
                }
                ReservedTours reservation = new ReservedTours();
                ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = reservation;
            }
        }
    }
}
