using Model.Comments;
using Service.Exceptions;
using Service.Posts;
using System.Xml.Linq;

namespace Service.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostService _postService;

        private const string PostNotFoundMessage = "Post with Id not found";
        private const string CommentNotFoundInPostMessage = "Comment not found in Post";

        public CommentService(
            ICommentRepository commentRepository,
            IPostService postService) 
        {
            _commentRepository = commentRepository;
            _postService = postService;
        }

        public Comment Create(Comment comment)
        {
            ValidatePostId(comment.PostId);

            comment.CreationDate = DateTime.UtcNow;

            return _commentRepository.Create(comment);
        }

        public bool Delete(Guid id)
        {
            return _commentRepository.Delete(id);
        }

        public Comment Get(Guid id)
        {
            return _commentRepository.Get(id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _commentRepository.GetAll();
        }

        public IEnumerable<Comment> GetByPostId(Guid postId)
        {
            return _commentRepository.GetByPostId(postId);
        }

        public Comment Update(Comment comment)
        {
            var entity = ValidatePostIdInComment(comment.PostId, comment.Id);

            entity.Content = comment.Content;
            entity.Author = comment.Author;
            entity.UpdateDate = DateTime.UtcNow;

            return _commentRepository.Update(entity);
        }

        private void ValidatePostId(Guid postId)
        {
            if (_postService.Get(postId) is null)
            {
                throw new EntityNotFoundException($"{PostNotFoundMessage}: {postId}");
            }
        }

        private Comment ValidatePostIdInComment(Guid postId, Guid commentId)
        {
            var comment = _commentRepository.GetByPostIdAndCommentId(postId, commentId);

            if (comment is null)
            {
                throw new EntityNotFoundException($"{CommentNotFoundInPostMessage}: {postId}");
            }

            return comment;
        }
    }
}
