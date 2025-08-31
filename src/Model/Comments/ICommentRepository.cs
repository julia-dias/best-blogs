using System;
using System.Collections.Generic;

namespace Model.Comments
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAll();

        Comment Get(Guid id);

        Comment Create(Comment comment);

        Comment Update(Comment comment);

        bool Delete(Guid id);

        bool DeleteByPostId(Guid postId);

        IEnumerable<Comment> GetByPostId(Guid postId);

        Comment GetByPostIdAndCommentId(Guid postId, Guid commentId);
    }
}
