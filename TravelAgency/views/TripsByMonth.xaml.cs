using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for TripsByMonth.xaml
    /// </summary>
    public partial class TripsByMonth : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Trip> trips;
        public ObservableCollection<Trip> Trips
        {
            get { return trips; }
            set
            {
                trips = value;
                OnPropertyChanged();
            }
        }

        public TripsByMonth()
        {
            InitializeComponent();
            DataContext = this;
            Trips = null;
        }

        private ObservableCollection<Trip> GetTrips(int month, int year)
        {
            var converter = new Base64StringToImageSourceConverter();
            ObservableCollection<Trip> trips = new ObservableCollection<Trip>();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                foreach (Tour tour in dbContext.Tours)
                {
                    if (tour.From.Month == month && tour.From.Year == year)
                    {
                        trips.Add(new Trip
                        {
                            Location = tour.Name,
                            DateRange = tour.From.ToString("d") + " - " + tour.To.ToString("d") + " (" + (int)(tour.To - tour.From).TotalDays + " dana)",
                            Price = tour.Price,
                            Id = tour.Id,
                            Image = (BitmapImage)converter.Convert(tour.Picture, null, null, null)
                        });
                    }
                }
            }
            else
            {
                MessageBox.Show("Error occurred while accessing the database.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return trips;
        }

        private void TripListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tripListBox.SelectedItem != null)
            {
                var selectedTripId = ((Trip)tripListBox.SelectedItem).Id;
                TourDetailsAgent tourDetails = new TourDetailsAgent(selectedTripId);
                AgentMainWindow agentMainWindow = (AgentMainWindow)Application.Current.MainWindow;
                agentMainWindow.contentControl.Content = tourDetails;
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = datePicker.SelectedDate ?? DateTime.MinValue;
            int month = selectedDate.Month;
            int year = selectedDate.Year;

            Trips = GetTrips(month, year);
        }

        // Implement INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
