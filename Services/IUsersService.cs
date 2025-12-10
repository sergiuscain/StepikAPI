using System.Data;

public interface IUsersService
{
    bool Add(User user);
    string FormatUserMetrics(int number);
    User Get(string fullName);
    int GetTotalCount();
    DataSet GetUserRating();
    DataSet GetUserSocialInfo(string userName);
}