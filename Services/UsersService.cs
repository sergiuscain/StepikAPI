using MySql.Data.MySqlClient;
using System.Data;

public class UsersService
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
            //Создаем подключение
            using MySqlConnection connection = new MySqlConnection(Constant.ConnectionString);
            connection.Open();
            Console.WriteLine("Connection open");
            // Вставка данных
            using var command = new MySqlCommand(@"
            INSERT INTO users (full_name, details, join_date, avatar, is_active) 
            VALUES (@full_name, @details, @join_date, @avatar, @is_active);", connection);
            command.Parameters.AddWithValue("@full_name", user.full_name);
            command.Parameters.AddWithValue("@details", user.details);
            command.Parameters.AddWithValue("@join_date", user.join_date);
            command.Parameters.AddWithValue("@avatar", user.avatar);
            command.Parameters.AddWithValue("@is_active", user.is_active);

            int rowsAffected = command.ExecuteNonQuery();
            Console.WriteLine($"Успешно добавлен пользователь...{rowsAffected}");

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось добавить пользователя: {ex.Message}");
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
                full_name = reader.GetString("full_name"),
                details = reader.IsDBNull("details") ? null : reader.GetString("details"),
                join_date = reader.GetDateTime("join_date"),
                avatar = reader.IsDBNull("avatar") ? null : reader.GetString("avatar"),
                is_active = reader.GetBoolean("is_active"),
                knowledge = reader.GetInt32("knowledge"),
                reputation = reader.GetInt32("reputation"),
                followers_count = reader.GetInt32("followers_count")
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