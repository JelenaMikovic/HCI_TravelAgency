using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.db;
using TravelAgency.repository.IRepository;

namespace TravelAgency.repository
{
    internal class RestaurantRepository : IRestaurauntRepository
    {
        private readonly DbContext _dbContext;

        public RestaurantRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
