using Model.Posts;
using Service.Comments;
using Service.Exceptions;
using Microsoft.Extensions.Logging;

namespace Service.Posts
{
    public class PostService : IPostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly IPostRepository _postRepository;
        private readonly ICommentService _commentService;

        private const string PostNotFoundMessage = "Post with Id not found";
        private const string DeletedCommentsCascadeMessage = "Deleted comments associated with Post";

        public PostService(
            ILogger<PostService> logger,
            IPostRepository postRepository,
            ICommentService commentService)
        {
            _logger = logger;
            _postRepository = postRepository;
            _commentService = commentService;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            post.CreationDate = DateTime.UtcNow;

            return await _postRepository.CreateAsync(post);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var commentsDeleted = await _commentService.DeleteByPostIdAsync(id);
            if (commentsDeleted)
            {
                _logger.LogInformation($"{DeletedCommentsCascadeMessage}: {id}");
            }

            return await _postRepository.DeleteAsync(id);
        }

        public async Task<Post> GetAsync(Guid id)
        {
            return await _postRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _postRepository.GetAllAsync();
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            var entity = await GetAsync(post.Id)
                ?? throw new EntityNotFoundException($"{PostNotFoundMessage}: {post.Id}");

            entity.Title = post.Title;
            entity.Content = post.Content;
            entity.UpdateDate = DateTime.UtcNow;

            return await _postRepository.UpdateAsync(entity);
        }
    }
}
