using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for EditTour.xaml
    /// </summary>
    public partial class EditTour : UserControl
    {
        private int selectedTripId;
        private Trip SelectedTrip;

        public EditTour(int selectedTripId)
        {
            this.selectedTripId = selectedTripId;
            InitializeComponent();
            SelectedTrip = GetTrip();
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

                    from.SelectedDate = tour.From;
                    to.SelectedDate = tour.To;
                    nametxt.Text = tour.Name;
                    citytxt.Text = tour.StartingLocation.City;
                    pricetxt.Text = tour.Price.ToString();
                    DraggedImage.Source = found.Image;
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return found;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length > 0)
                {
                    string imagePath = files[0];
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

        private void Save(object sender, RoutedEventArgs e)
        {
            int price;
            if (int.TryParse(pricetxt.Text, out price))
            {
                
            
            MessageBoxResult result = MessageBox.Show("Molimo Vas da potvrdite promene.", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                if (Application.Current.Resources["DbContext"] is DbContext dbContext)
                {
                    Tour attraction = dbContext.Tours.Find(selectedTripId);

                    Location location = dbContext.Locations.Find(attraction.StartingLocation.Id);
                    Location newLocation = new Location
                    {
                        Id = location.Id,
                        Address = location.Address,
                        City = citytxt.Text,
                        Country = location.Country
                    };
                    dbContext.Locations.Remove(location);
                    var converter = new Base64StringToImageSourceConverter();
                    dbContext.Locations.Add(newLocation);
                    Tour updated = new Tour
                    {
                        Id = attraction.Id,
                        From = (DateTime)from.SelectedDate,
                        To = (DateTime)to.SelectedDate,
                        Price = price,
                        Name = nametxt.Text,
                        Picture = (string)converter.ConvertBack(DraggedImage.Source, null, null, null),
                        StartingLocation = newLocation
                    };
                    dbContext.Tours.Remove(attraction);
                    dbContext.Tours.Add(updated);
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
            else
            {
                MessageBox.Show("Cena nije validna!", "Pogresan unos.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            EditTourMain tourDetails = new EditTourMain(selectedTripId);
            AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = tourDetails;
        }

        private void ShowHelp_Click(object sender, RoutedEventArgs e)
        {
            Help display = new Help("/../../../help/EditTour.html");
            display.ShowDialog();
        }
    }
}
