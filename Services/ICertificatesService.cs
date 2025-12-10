using System.Data;

namespace API.Services
{
    public interface ICertificatesService
    {
        DataSet Get(string fullName);
    }
}