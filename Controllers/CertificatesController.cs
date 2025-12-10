using API.Services;
using API.Services.ADO.NET;
using Microsoft.AspNetCore.Mvc;
using System.Data;

[ApiController]
[Route("[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly ICertificatesService _certificatesService;

    public CertificatesController(ICertificatesService certificatesService)
    {
        _certificatesService = certificatesService;
    }
    [HttpGet("GetUserCertificates")]
    public IActionResult GetUserCertificates(string fullName)
    {
        var dataSet = _certificatesService.Get(fullName);
        var certificates = dataSet.Tables[0].Rows.Cast<DataRow>().Select(row => new
        {
            CourseTitle = row["title"].ToString(),
            IssueDate = Convert.ToDateTime(row["issue_date"]),
            Grade = Convert.ToInt32(row["grade"])
        }).ToList();

        return (certificates != null && certificates.Any()) ? Ok(certificates) : NotFound("У пользователя не найдено курсов");
    }
}
