﻿using System;
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
    /// Interaction logic for EditAttractionMain.xaml
    /// </summary>
    public partial class EditAttractionMain : UserControl
    {
        private int selectedTripId;
        private int restaurantId;
        public TripAttraction SelectedAttraction { get; set; }
        public EditAttractionMain(int selectedTripId, int restaurantId)
        {
            this.selectedTripId = selectedTripId;
            this.restaurantId = restaurantId;
            InitializeComponent();
            DataContext = this;
            SelectedAttraction = GetRestaurant();
        }

        private TripAttraction GetRestaurant()
        {
            var converter = new Base64StringToImageSourceConverter();
            TripAttraction Restaurant = new TripAttraction();
            if (Application.Current.Resources["DbContext"] is DbContext dbContext)
            {
                Attraction attraction = dbContext.Attractions.Find(restaurantId);
                Restaurant = new TripAttraction
                {
                    Location = "Adresa: " + attraction.Location.Address,
                    Id = attraction.Id,
                    TourID = attraction.TourID,
                    Description = attraction.Description,
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

        }

        private void Delete(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            EditTourMain tourDetails = new EditTourMain(selectedTripId);
            AgentMainWindow clientMainWindow = (AgentMainWindow)Application.Current.MainWindow;
            clientMainWindow.contentControl.Content = tourDetails;
        }
    }
}
