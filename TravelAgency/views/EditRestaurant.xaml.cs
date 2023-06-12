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
    /// Interaction logic for EditRestaurant.xaml
    /// </summary>
    public partial class EditRestaurant : UserControl
    {
        private int selectedTripId;
        private int restaurantId;
        public TripRestaurant selectedRestaurant;
        public EditRestaurant(int selectedTripId,int restaurantId)
        {
            InitializeComponent();
            this.selectedTripId = selectedTripId;
            this.restaurantId = restaurantId;
            selectedRestaurant = GetAccomondation();
        }

        private TripRestaurant GetAccomondation()
        {
            var converter = new Base64StringToImageSourceConverter();
            TripRestaurant Attraction = new TripRestaurant();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Restaurant attraction = dbContext.Restaurants.Find(restaurantId);
                Attraction = new TripRestaurant
                {
                    Location = attraction.Location.Address,
                    Id = attraction.Id,
                    TourID = attraction.TourID,
                    Description = attraction.Description,
                    Name = attraction.Name,
                    Image = (BitmapImage)converter.Convert(attraction.Picture, null, null, null)
                };
                DraggedImage.Source = Attraction.Image;
                DraggedImage.Visibility = Visibility.Visible;
                adrestxt.Text = Attraction.Location;
                nametxt.Text = Attraction.Name;
                desctxt.Text = Attraction.Description;
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return Attraction;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string imagePath = files[0]; // Get the first file path
                    BitmapImage bitmap = new BitmapImage(new Uri(imagePath));
                    DraggedImage.Source = bitmap;
                    DraggedImage.Visibility = Visibility.Visible;
                    DragLabel.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditRestaurantMain tourDetails = new EditRestaurantMain(selectedTripId,restaurantId);
            AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = tourDetails;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Restaurant attraction = dbContext.Restaurants.Find(restaurantId);

                Location location = dbContext.Locations.Find(attraction.Location.Id);
                Location newLocation = new Location
                {
                    Id = location.Id,
                    Address = adrestxt.Text,
                    City = location.City,
                    Country = location.Country
                };
                dbContext.Locations.Remove(location);
                var converter = new Base64StringToImageSourceConverter();
                dbContext.Locations.Add(newLocation);
                Restaurant updated = new Restaurant
                {
                    Id = attraction.Id,
                    TourID = attraction.TourID,
                    Name = nametxt.Text,
                    Description = desctxt.Text,
                    Picture = (string)converter.ConvertBack(DraggedImage.Source, null, null, null),
                    Location = newLocation
                };
                dbContext.Restaurants.Remove(attraction);
                dbContext.Restaurants.Add(updated);
                EditRestaurantMain tourDetails = new EditRestaurantMain(selectedTripId, restaurantId);
                AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                clientMainWindow.contentControl.Content = tourDetails;
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
