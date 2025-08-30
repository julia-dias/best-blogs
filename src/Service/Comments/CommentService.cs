using Model.Comments;
using Service.Posts;
using System.Xml.Linq;

namespace Service.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostService _postService;

        private const string PostNotFoundMessage = "PostId not found.";

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
            ValidatePostId(comment.PostId);

            return _commentRepository.Update(comment);
        }

        private void ValidatePostId(Guid postId)
        {
            if (_postService.Get(postId) is null)
            {
                throw new InvalidOperationException(PostNotFoundMessage);
            }
        }
    }
}
