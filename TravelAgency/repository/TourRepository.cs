using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.db;
using TravelAgency.repository.IRepository;

namespace TravelAgency.repository
{
    public class TourRepository : ITourRepository
    {
        private readonly DbContext _dbContext;

        public TourRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
