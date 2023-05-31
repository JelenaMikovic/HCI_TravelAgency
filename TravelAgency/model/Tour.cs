using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location StartingLocation { get; set; }
        public string Picture { get; set; }
        public int Price { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool isDeleted { get; set; }
    }
}
