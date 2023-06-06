using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.db;
using TravelAgency.repository.IRepository;

namespace TravelAgency.repository
{
    public class AttractionRepository : IAttractionRepository
    {
        private readonly DbContext _dbContext;

        public AttractionRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
