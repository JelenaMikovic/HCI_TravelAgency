using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.db;
using TravelAgency.repository.IRepository;

namespace TravelAgency.repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly DbContext _dbContext;

        public LocationRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
