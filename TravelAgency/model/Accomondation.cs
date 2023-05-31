using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.model
{
    public class Accomondation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public string Picture { get; set; }
        public AccomondationType Type { get; set; }
        public Tour Tour { get; set; }
        public bool isDeleted { get; set; }
    }
}
