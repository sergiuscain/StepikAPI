using API.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.EF
{
    public class CertificatesService : ICertificatesService
    {
        private readonly ApplicationDbContext dbContext;
        public CertificatesService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public DataSet Get(string fullName)
        {
            throw new NotImplementedException();
        }
    }
}
