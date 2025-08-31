using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.Comments
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();

        Task<Comment> GetAsync(Guid id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment> UpdateAsync(Comment comment);

        Task<bool> DeleteAsync(Guid id);

        Task<bool> DeleteByPostIdAsync(Guid postId);

        Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId);

        Task<Comment> GetByPostIdAndCommentIdAsync(Guid postId, Guid commentId);
    }
}
