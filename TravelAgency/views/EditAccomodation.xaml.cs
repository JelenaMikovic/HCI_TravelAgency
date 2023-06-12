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
    /// Interaction logic for EditAccomodation.xaml
    /// </summary>
    public partial class EditAccomodation : UserControl
    {
        private int selectedTripId;
        private int restaurantId;
        public TripAccomodation selectedAccomodation;

        public EditAccomodation(int selectedTripId, int restaurantId)
        {
            InitializeComponent();
            this.selectedTripId = selectedTripId;
            this.restaurantId = restaurantId;
            selectedAccomodation = GetAccomondation();
        }

        private TripAccomodation GetAccomondation()
        {
            var converter = new Base64StringToImageSourceConverter();
            TripAccomodation Attraction = new TripAccomodation();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Accomondation attraction = dbContext.Accomondations.Find(restaurantId);
                Attraction = new TripAccomodation
                {
                    Location = attraction.Location.Address,
                    Id = attraction.Id,
                    TourID = attraction.TourID,
                    Type = attraction.Type.ToString(),
                    Name = attraction.Name,
                    Image = (BitmapImage)converter.Convert(attraction.Picture, null, null, null)
                };
                DraggedImage.Source = Attraction.Image;
                DraggedImage.Visibility = Visibility.Visible;
                adrestxt.Text = Attraction.Location;
                nametxt.Text = Attraction.Name;
                myComboBox.SelectedIndex = ((int)attraction.Type);
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
            EditAccomodationMain tourDetails = new EditAccomodationMain(selectedTripId,restaurantId);
            AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = tourDetails;
        }
    }
}
