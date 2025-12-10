using MySql.Data.MySqlClient;

public class CoursesService
{
    /// <summary>
    /// Получение общего количества курсов
    /// </summary>
    public int GetTotalCount()
    {
        using var connection = new MySqlConnection(Constant.ConnectionString);
        connection.Open();
        using var command = new MySqlCommand("SELECT COUNT(*) FROM courses;", connection);
        var result = command.ExecuteScalar();
        return result == null ? 0 : Convert.ToInt32(result);
    }
    /// <summary>
    /// Получение списка курсов пользователя
    /// </summary>
    /// <param name="fullName">Полное имя пользователя</param>
    /// <returns>Список курсов</returns>
    public List<Course> Get(string fullName)
    {
        var courses = new List<Course>();

        using var connection = new MySqlConnection(Constant.ConnectionString);
        connection.Open();

        // Используем JOIN для получения всех курсов пользователя
        using var command = new MySqlCommand(@"SELECT c.* 
                                    FROM courses c
                                    INNER JOIN user_courses uc ON c.id = uc.course_id
                                    INNER JOIN users u ON uc.user_id = u.id
                                    WHERE u.full_name = @full_name 
                                    AND u.is_active = true;", connection);

        command.Parameters.Add(new MySqlParameter("@full_name", fullName));

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var course = new Course()
            {
                Title = reader.GetString("title"),
                Summary = reader.IsDBNull(reader.GetOrdinal("Summary"))
                                ? null
                                : reader.GetString("Summary"),
                Photo = reader.IsDBNull(reader.GetOrdinal("photo"))
                        ? null
                        : reader.GetString("photo")
            };

            courses.Add(course);
        }

        return courses;
    }
}