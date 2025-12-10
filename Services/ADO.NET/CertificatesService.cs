using MySql.Data.MySqlClient;
using System.Data;


namespace API.Services.ADO.NET
{
    public class CertificatesService : ICertificatesService
    {
        /// <summary>
        /// Получение сертификатов пользователя
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>DataSet</returns>
        public DataSet Get(string fullName)
        {
            // Создаем подключение, DataSet, SQL запрос, объект команды и DataAdapter
            using var connection = new MySqlConnection(Constant.ConnectionString);
            var dataSet = new DataSet();
            string sqlQuery = $@"SELECT courses.title, certificates.issue_date, certificates.grade
                                FROM certificates
                                JOIN users ON certificates.user_id = users.id
                                JOIN courses ON certificates.course_id = courses.id
                                WHERE users.full_name = @fullName
                                ORDER BY certificates.issue_date DESC;";
            using var command = new MySqlCommand(sqlQuery, connection);
            // Имя передается через параметр команды, что бы защитить от инъекции чужого SQL кода
            command.Parameters.AddWithValue("@fullName", fullName);
            using var dataAdapter = new MySqlDataAdapter(command);
            // Заполняем DataSet
            dataAdapter.Fill(dataSet);
            return dataSet;
        }
    }
}
