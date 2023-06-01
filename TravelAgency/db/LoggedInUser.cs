using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.model;

namespace TravelAgency.db
{
    public static class LoggedInUser
    {
        public static User CurrentUser { get; set; }
    }
}
