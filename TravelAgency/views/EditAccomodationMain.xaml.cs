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
using TravelAgency.converters;
using TravelAgency.db;
using TravelAgency.model;

namespace TravelAgency.views
{
    /// <summary>
    /// Interaction logic for EditAccomodationMain.xaml
    /// </summary>
    public partial class EditAccomodationMain : UserControl
    {
        private int selectedTripId;
        private int restaurantId;
        public TripAccomodation SelectedAccomodation { get; set; }
        public EditAccomodationMain(int selectedTripId, int restaurantId)
        {
            this.selectedTripId = selectedTripId;
            this.restaurantId = restaurantId;
            InitializeComponent();
            DataContext = this;
            SelectedAccomodation = GetRestaurant();
        }

        private TripAccomodation GetRestaurant()
        {
            var converter = new Base64StringToImageSourceConverter();
            TripAccomodation Restaurant = new TripAccomodation();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Accomondation attraction = dbContext.Accomondations.Find(restaurantId);
                Restaurant = new TripAccomodation
                {
                    Location = "Adresa: " + attraction.Location.Address,
                    Id = attraction.Id,
                    TourID = attraction.TourID,
                    Type = attraction.Type.ToString(),
                    Name = attraction.Name,
                    Image = (BitmapImage)converter.Convert(attraction.Picture, null, null, null)
                };
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Restaurant;
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            EditAccomodation tourDetails = new EditAccomodation(selectedTripId,restaurantId);
            AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = tourDetails;
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Molimo Vas da potvrdite brisanje.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Application.Current.Resources["DbContext"] is DbContext dbContext)
                {
                    Accomondation attraction = dbContext.Accomondations.Find(restaurantId);
                    Location location = dbContext.Locations.Find(attraction.Location.Id);
                    dbContext.Locations.Remove(location);
                    dbContext.Accomondations.Remove(attraction);
                    EditTourMain tourDetails = new EditTourMain(selectedTripId);
                    AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                    clientMainWindow.contentControl.Content = tourDetails;
                }
                else
                {
                    MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditTourMain tourDetails = new EditTourMain(selectedTripId);
            AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = tourDetails;
        }
    }
}
