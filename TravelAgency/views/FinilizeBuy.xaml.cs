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
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da odustanete?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TourDetails tourDetails = new TourDetails(detailedTrip.Id);
                ClientMainWindow clientMainWindow = (ClientMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = tourDetails;
            }
        }

        private void Buy(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Molimo Vas da potvrdite kupovinu.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Application.Current.Resources["DbContext"] is DbContext dbContext)
                {
                    foreach (ReservedTour tour in dbContext.ReservedTours)
                    {
                        if (tour.TourId == detailedTrip.Id && LoggedInUser.CurrentUser.Id == tour.UserId)
                        {
                            MessageBox.Show("Vec ste rezervisali ovo putovanje!", "Neuspela rezervaija", MessageBoxButton.OK, MessageBoxImage.Warning);
                            ReservedTours reservationn = new ReservedTours();
                            ClientMainWindow clientMainWindown = (ClientMainWindow)Application.Current.MainWindow;
                            clientMainWindown.contentControl.Content = reservationn;
                            return;
                        }
                    }
                    foreach (BoughtTour tour in dbContext.BoughtTours)
                    {
                        if (tour.TourId == detailedTrip.Id && LoggedInUser.CurrentUser.Id == tour.UserId)
                        {
                            MessageBox.Show("Vec ste kupili ovo putovanje!", "Neuspela rezervaija", MessageBoxButton.OK, MessageBoxImage.Warning);
                            BoughtTours reservationn = new BoughtTours();
                            ClientMainWindow clientMainWindown = (ClientMainWindow)Application.Current.MainWindow;
                            clientMainWindown.contentControl.Content = reservationn;
                            return;
                        }
                    }
                    MessageBox.Show("Uspesno ste kupili putovanje!", "Obavljena upovina", MessageBoxButton.OK, MessageBoxImage.Information);
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
                        Id = dbContext.BoughtTours.Count() + 100,
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
}
