using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelAgency.converters;
using TravelAgency.db;
using TravelAgency.model;


namespace TravelAgency.views
{
    /// <summary>
    /// Interaction logic for CreateNewAttraction.xaml
    /// </summary>
    public partial class CreateNewAttraction : UserControl
    {
        private int tourId;
        public CreateNewAttraction(int selectedTour)
        {
            InitializeComponent();
            tourId = selectedTour;
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

        private void Back(object sender, RoutedEventArgs e)
        {
            EditTourMain futureTrips = new EditTourMain(tourId);
            AgentMainWindow agentMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            agentMainWindow.contentControl.Content = futureTrips;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Molimo Vas da potvrdite promene.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Application.Current.Resources["DbContext"] is DbContext dbContext)
                {
                    Tour attraction = dbContext.Tours.Find(tourId);

                    //Location location = dbContext.Locations.Find(attraction.Location.Id);
                    Location newLocation = new Location
                    {
                        Id = dbContext.Locations.Count() + 10,
                        Address = adrestxt.Text,
                        City = attraction.StartingLocation.City,
                        Country = attraction.StartingLocation.Country
                    };
                    //dbContext.Locations.Remove(location);
                    dbContext.Locations.Add(newLocation);
                    var converter = new Base64StringToImageSourceConverter();
                    Attraction updated = new Attraction
                    {
                        Id = dbContext.Attractions.Count() + 10,
                        TourID = attraction.Id,
                        Name = nametxt.Text,
                        Description = desctxt.Text,
                        Picture = (string)converter.ConvertBack(DraggedImage.Source, null, null, null),
                        Location = newLocation
                    };
                    //dbContext.Attractions.Remove(attraction);
                    dbContext.Attractions.Add(updated);
                    dbContext.SaveChanges();
                    EditTourMain tourDetails = new EditTourMain(tourId);
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
}
