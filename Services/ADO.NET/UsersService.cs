using MySql.Data.MySqlClient;
using System.Data;

namespace API.Services.ADO.NET
{
    public class UsersService : IUsersService
    {
        /// <summary>
        /// Добавление нового пользователя в таблицу users
        /// </summary>
        /// <param name="user">Новый пользователь</param>
        /// <returns>Удалось ли добавить пользователя</returns>
        public bool Add(User user)
        {
            try
            {
                using var connection = new MySqlConnection(Constant.ConnectionString);
                connection.Open();
                var query = @"
        INSERT INTO users (
            full_name,
            details,
            join_date,
            avatar,
            is_active,
            knowledge,
            reputation,
            followers_count,
            days_without_break,
            days_without_break_max,
            solved_tasks
        )
        VALUES (
            @FullName,
            @Details,
            @JoinDate,
            @Avatar,
            @IsActive,
            @Knowledge,
            @Reputation,
            @FollowersCount,
            @DaysWithoutBreak,
            @DaysWithoutBreakMax,
            @SolvedTasks
        )";

                using var command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Details", user.Details ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@JoinDate", user.JoinDate);
                command.Parameters.AddWithValue("@Avatar", user.Avatar ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", user.IsActive);
                command.Parameters.AddWithValue("@Knowledge", user.Knowledge);
                command.Parameters.AddWithValue("@Reputation", user.Reputation);
                command.Parameters.AddWithValue("@FollowersCount", user.FollowersCount);
                command.Parameters.AddWithValue("@DaysWithoutBreak", user.DaysWithoutBreak);
                command.Parameters.AddWithValue("@DaysWithoutBreakMax", user.DaysWithoutBreakMax);
                command.Parameters.AddWithValue("@SolvedTasks", user.SolvedTasks);

                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected == 1;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Получение пользователя из таблицы users
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>User</returns>
        public User Get(string fullName)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();
            using var command = new MySqlCommand($"SELECT * FROM users Where full_name = @full_name AND is_active = true", connection);
            command.Parameters.AddWithValue("@full_name", fullName);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var user = new User
                {
                    FullName = reader.GetString("full_name"),
                    Details = reader.IsDBNull("details") ? null : reader.GetString("details"),
                    JoinDate = reader.GetDateTime("join_date"),
                    Avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar"),
                    IsActive = reader.GetBoolean("is_active"),
                    Knowledge = reader.GetInt32("knowledge"),
                    Reputation = reader.GetInt32("reputation"),
                    FollowersCount = reader.GetInt32("followers_count")
                };
                return user;
            }
            //Пользователь не найден
            return null;
        }
        /// <summary>
        /// Получение общего количества пользователей
        /// </summary>
        public int GetTotalCount()
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();
            using var command = new MySqlCommand("SELECT COUNT(*) FROM users;", connection);
            var result = command.ExecuteScalar();
            return result == null ? 0 : Convert.ToInt32(result);
        }
        /// <summary>
        /// Форматирование показателей пользователя
        /// </summary>
        /// <param name="number">Число для форматирования</param>
        /// <returns>Отформатированное число</returns>
        public string FormatUserMetrics(int number)
        {
            using var connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();

            // Создание команды для вызова функции
            var functionName = "format_number";
            using var command = new MySqlCommand(functionName, connection);
            command.CommandType = CommandType.StoredProcedure;

            // Указываем параметр для возвращаемого значения
            var returnValueParam = new MySqlParameter()
            {
                Direction = ParameterDirection.ReturnValue
            };

            // Добавляем входной параметр
            var numberParam = new MySqlParameter("number", number)
            {
                Direction = ParameterDirection.Input
            };

            // Добавляем параметры к команде
            command.Parameters.Add(numberParam);
            command.Parameters.Add(returnValueParam);

            // Выполнение команды
            command.ExecuteNonQuery();

            // Получение значения возвращаемого параметра
            var returnValue = returnValueParam.Value;

            // Возвращаем значение
            return returnValue?.ToString() ?? "";
        }
        /// <summary>
        /// Рейтинг пользователей
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetUserRating()
        {
            // Создаем соединение.
            using var connection = new MySqlConnection(Constant.ConnectionString);
            // Создаем DataSet
            var dataSet = new DataSet();
            // Создаем текст запроса для выборки данных на SQL.
            var sqlQuery = @"SELECT full_name, knowledge, reputation 
                        FROM users
                        WHERE is_active = true 
                        ORDER BY knowledge DESC
                        LIMIT 10;";
            // Создаем DataAdapter, который будет заполнять DataSet.
            using var dataAdapter = new MySqlDataAdapter(sqlQuery, connection);
            // Заполняем DataSet данными на основе запроса, который мы передали в конструкторе DataAdapter-а
            dataAdapter.Fill(dataSet);
            // Возвращаем DataSet с заполненными данными.
            return dataSet;
        }
        /// <summary>
        /// Получение социальной информации пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <returns>DataSet</returns>
        public DataSet GetUserSocialInfo(string userName)
        {
            // Открываем соединение
            using var connection = new MySqlConnection(Constant.ConnectionString);
            // Создаем команду и передаем в неё запрос и соединение. Соединение откроется внутри метода adapter.Fill()
            using var command = new MySqlCommand("CALL get_user_social_info(@user_name);", connection);
            // Добавляем параметр для защиты от SQL инъекций
            command.Parameters.AddWithValue("@user_name", userName);
            // Создаем адаптер и DataSet
            using var adapter = new MySqlDataAdapter(command);
            var dataSet = new DataSet();
            // Заполняем DataSet. Соединение откроется без нашего участия.
            adapter.Fill(dataSet);
            // Возвращаем заполненный DataSet
            return dataSet;
        }
    }
}