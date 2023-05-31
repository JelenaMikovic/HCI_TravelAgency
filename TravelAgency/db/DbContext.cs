using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.db
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<ReservedTour> ReservedTours { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<BoughtTour> BoughtTours { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<Accomondation> Accomondations { get; set; }

        public void InitDataBase()
        {

        }

    }
}
