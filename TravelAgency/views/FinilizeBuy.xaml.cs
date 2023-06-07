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
    /// Interaction logic for FinilizeBuy.xaml
    /// </summary>
    public partial class FinilizeBuy : UserControl
    {
        public List<TripAttraction> attractions { get; set; }
        public List<TripAccomodation> accomondations { get; set; }
        public List<TripRestaurant> restaurants { get; set; }
        public Trip detailedTrip { get; set; }

        public FinilizeBuy(List<TripAttraction> selctedAttractions, List<TripAccomodation> selectedAccomondation, List<TripRestaurant> selectedRestaurants, Trip detailedTrip)
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

        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Uspesno ste kupili putovanje!", "Obavljena upovina", MessageBoxButton.OK, MessageBoxImage.Information);
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
                dbContext.BoughtTours.Add(new BoughtTour
                {
                    Id = dbContext.ReservedTours.Count() + 200,
                    TourId = detailedTrip.Id,
                    UserId = LoggedInUser.CurrentUser.Id,
                    Attractions = atr,
                    Restaurants = res,
                    Accomondation = acc,
                    isDeleted = false
                });

                dbContext.SaveChanges();
            }
            BoughtTours reservation = new BoughtTours();
            ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = reservation;
        }
    }
}
