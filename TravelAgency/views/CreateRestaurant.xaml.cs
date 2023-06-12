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
using TravelAgency.model;

namespace TravelAgency.views
{
    /// <summary>
    /// Interaction logic for CreateRestaurant.xaml
    /// </summary>
    public partial class CreateRestaurant : UserControl
    {
        private int tourId;
        public CreateRestaurant(int selectedTour)
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
    }
}
