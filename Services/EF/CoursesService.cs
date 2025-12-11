using API.Data;
using MySql.Data.MySqlClient;

namespace API.Services.EF
{
    public class CoursesService : ICoursesService
    {
        private readonly ApplicationDbContext dbContext;
        public CoursesService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public List<Course> Get(string fullName)
        {
            throw new NotImplementedException();
        }

        public int GetTotalCount()
        {
            throw new NotImplementedException();
        }
    }
}