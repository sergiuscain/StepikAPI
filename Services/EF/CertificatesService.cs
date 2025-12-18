using API.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.Common;

namespace API.Services.EF
{
    public class CertificatesService : ICertificatesService
    {
        private readonly ApplicationDbContext dbContext;
        public CertificatesService(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        /// <summary>
        /// Получение сертификатов пользователя
        /// </summary>
        /// <param name="fullName">Полное имя пользователя</param>
        /// <returns>DataSet</returns>
        public DataSet Get(string fullName)
        {

            var certificates = dbContext.Certificates
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Course)
                .Where(c => c.User.FullName == fullName)
                .OrderByDescending(c => c.IssueDate)
                .Select(c => new
                {
                    CourseTitle = c.Course.Title,
                    IssueDate = c.IssueDate,
                    Grade = c.Grade
                })
                .ToList();

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("title", typeof(string));
            dataTable.Columns.Add("issue_date", typeof(DateTime));
            dataTable.Columns.Add("grade", typeof(int));

            foreach (var cert in certificates)
            {
                dataTable.Rows.Add(cert.CourseTitle, cert.IssueDate, cert.Grade);
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }
    }
}
