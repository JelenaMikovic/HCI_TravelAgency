using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TravelAgency.db;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var dbContext = new DbContext();
            dbContext.InitDataBase();
            Resources["DbContext"] = dbContext;
        }
    }
}
