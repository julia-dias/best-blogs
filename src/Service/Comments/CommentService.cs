using Model.Comments;

namespace Service.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository) 
        {
            _commentRepository = commentRepository;
        }

        public Comment Create(Comment comment)
        {
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
            return _commentRepository.Update(comment);
        }
    }
}
