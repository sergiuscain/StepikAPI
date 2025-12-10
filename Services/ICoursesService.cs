
public interface ICoursesService
{
    List<Course> Get(string fullName);
    int GetTotalCount();
}