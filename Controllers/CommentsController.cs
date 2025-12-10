using API.Services.ADO.NET;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class CommentsController : ControllerBase
{
    private readonly ICommentsService _commentsService;

    public CommentsController(ICommentsService commentsService)
    {
        _commentsService = commentsService;
    }

    [HttpGet("GetCourseComments")]
    public IActionResult GetCourseComments(int id)
    {
        var comments = _commentsService.Get(id);
        return (comments != null && comments.Any()) ? Ok(comments) : NotFound("Комментариев не найдено");
    }

    [HttpDelete("DeleteComment")]
    public IActionResult DeleteComment(int id)
    {
        var result = _commentsService.Delete(id);
        return result ? Ok("Комментарий удален") : BadRequest("Не удалось удалить комментарий.");
    }
}
