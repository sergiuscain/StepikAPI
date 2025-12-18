using API.Data;
using Microsoft.EntityFrameworkCore;
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
        /// <summary>
        /// Получение списка курсов пользователя
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>Список курсов</returns>
        public List<Course> Get(string fullName)
        {
            var courses = dbContext.UserCourses
                .AsNoTracking()
                .Include(uc => uc.User)
                .Include(uc => uc.Course)
                .Where(uc => uc.User.FullName == fullName && uc.User.IsActive == true)
                .OrderByDescending(uc => uc.LastViewed)
                .Select(uc => uc.Course)
                .ToList();

            return courses;
        }
        /// <summary>
        /// Получение общего количества курсов
        /// </summary>
        public int GetTotalCount()
        {
            return dbContext.Courses.Count();
        }
    }
}