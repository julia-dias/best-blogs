using Model.Comments;
using Service.Exceptions;
using Service.Posts;
using Service.Posts.Validators;

namespace Service.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostValidator _postValidator;

        private const string CommentNotFoundInPostMessage = "Comment not found in Post";

        public CommentService(
            ICommentRepository commentRepository,
            IPostValidator postValidator) 
        {
            _commentRepository = commentRepository;
            _postValidator = postValidator;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _postValidator.ValidatePostExistsAsync(comment.PostId);

            comment.CreationDate = DateTime.UtcNow;

            return await _commentRepository.CreateAsync(comment);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _commentRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteByPostIdAsync(Guid postId)
        {
            return await _commentRepository.DeleteByPostIdAsync(postId);
        }

        public async Task<Comment> GetAsync(Guid id)
        {
            return await _commentRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _commentRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            return await _commentRepository.GetByPostIdAsync(postId);
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var entity = await ValidatePostIdInCommentAsync(comment.PostId, comment.Id);

            entity.Content = comment.Content;
            entity.Author = comment.Author;
            entity.UpdateDate = DateTime.UtcNow;

            return await _commentRepository.UpdateAsync(entity);
        }

        private async Task<Comment> ValidatePostIdInCommentAsync(Guid postId, Guid commentId)
        {
            var comment = await _commentRepository.GetByPostIdAndCommentIdAsync(postId, commentId);

            if (comment is null)
            {
                throw new EntityNotFoundException($"{CommentNotFoundInPostMessage}: {postId}");
            }

            return comment;
        }
    }
}
