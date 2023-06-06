using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class BoughtTour
    {
        public int Id { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }
        public List<Attraction> Attractions { get; set; }
        public List<Restaurant> Restaurants { get; set; }
        public Accomondation Accomondation { get; set; }
        public bool isDeleted { get; set; }
    }
}
