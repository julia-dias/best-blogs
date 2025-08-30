using Model.Comments;

namespace Service.Comments
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAll();

        Comment Get(Guid id);

        Comment Create(Comment comment);

        Comment Update(Comment comment);

        bool Delete(Guid id);

        IEnumerable<Comment> GetByPostId(Guid postId);
    }
}
