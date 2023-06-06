using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.db;
using TravelAgency.repository.IRepository;

namespace TravelAgency.repository
{
    internal class ReservedTourRepository : IReservedTourRepository
    {
        private readonly DbContext _dbContext;

        public ReservedTourRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
