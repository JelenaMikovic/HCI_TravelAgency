using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.db;
using TravelAgency.model;
using TravelAgency.repository.IRepository;

namespace TravelAgency.repository
{
    public class AccomodationRepository  : IAccomodationRepository
    {
        private readonly DbContext _dbContext;

        public AccomodationRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Accomondation GetById(int id)
        {
            return _dbContext.Accomondations.Find(id);
        }
    }
}
