using API.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Globalization;

namespace API.Services.EF
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext dbContext;
        public UsersService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        /// <summary>
        /// Добавление нового пользователя в таблицу users
        /// </summary>
        /// <param name="user">Новый пользователь</param>
        /// <returns>Удалось ли добавить пользователя</returns>
        public bool Add(User user)
        {
            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Форматирование показателей пользователя
        /// </summary>
        /// <param name="number">Число для форматирования</param>
        /// <returns>Отформатированное число</returns>
        public string? FormatUserMetrics(int number)
        {
            if (number >= 1000)
            {
                double thousands = number / 1000.0;

                // Если число кратно 1000, показываем без десятичной части
                if (number % 1000 == 0)
                    return $"{(int)thousands}K";
                else
                    return $"{thousands:0.#}K"; // Один знак после запятой
            }
            else
            {
                return number.ToString();
            }
        }
        /// <summary>
        /// Получение пользователя из таблицы users
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>User</returns>
        public User Get(string fullName)
        {
            return dbContext.Users.Where(u => u.full_name == fullName && u.is_active).FirstOrDefault();
        }
        /// <summary>
        /// Получение общего количества пользователей
        /// </summary>
        public int GetTotalCount()
        {
            return dbContext.Users.Count();
        }

        public DataSet GetUserRating()
        {
            // return dbContext.Users.OrderBy(u => u.knowledge).Take(10);
            throw new NotImplementedException();
        }

        public DataSet GetUserSocialInfo(string userName)
        {
            throw new NotImplementedException();
        }
    }
}