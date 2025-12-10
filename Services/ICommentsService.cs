
public interface ICommentsService
{
    bool Delete(int id);
    List<Comment> Get(int id);
}