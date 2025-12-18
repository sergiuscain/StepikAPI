using API.Data;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Удаление комментария пользователя
        /// </summary>
        /// <param name="id">id комментария</param>
        /// <returns>Удалось ли удалить комментарий</returns>
        public bool Delete(int id)
        {
            try
            {

                var courseReviews = dbContext.CourseReviews.Where(cr => cr.CommentId == id);
                dbContext.CourseReviews.RemoveRange(courseReviews);

                var replies = dbContext.Comments.Where(c => c.ReplyCommentId == id);
                dbContext.Comments.RemoveRange(replies);

                var comment = dbContext.Comments.FirstOrDefault(c => c.Id == id);
                if (comment != null)
                {
                    dbContext.Comments.Remove(comment);
                }

                dbContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Получение всех комментариев к курсу
        /// </summary>
        /// <param name="id">id курса</param>
        /// <returns>Список комментариев</returns> 
        public List<Comment> Get(int id)
        {
            var comments = dbContext.Comments
                .AsNoTracking()
                .Where(c =>
                    c.ReplyCommentId == null &&
                    c.Step.Lesson.UnitLessons.Any(ul => ul.Unit.CourseId == id)
                )
                .OrderByDescending(c => c.Time)
                .ToList();

            return comments;
        }
    }
}