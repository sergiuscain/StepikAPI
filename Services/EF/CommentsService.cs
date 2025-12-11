using API.Data;
using MySql.Data.MySqlClient;

namespace API.Services.EF
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationDbContext dbContext;
        public CommentsService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Comment> Get(int id)
        {
            throw new NotImplementedException();
        }
    }
}