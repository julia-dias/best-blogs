using Model.Comments;

namespace Service.Comments
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllAsync();

        Task<Comment> GetAsync(Guid id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment> UpdateAsync(Comment comment);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> DeleteByPostIdAsync(Guid postId);

        Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);
    }
}
