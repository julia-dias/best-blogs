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

        public Post Create(Post post)
        {
            post.CreationDate = DateTime.UtcNow;

            return _postRepository.Create(post);
        }

        public bool Delete(Guid id)
        {
            var commentsDeleted = _commentService.DeleteByPostId(id);
            if (commentsDeleted)
            {
                _logger.LogInformation($"{DeletedCommentsCascadeMessage}: {id}");
            }

            return _postRepository.Delete(id);
        }

        public Post Get(Guid id)
        {
            return _postRepository.Get(id);
        }

        public IEnumerable<Post> GetAll()
        {
            return _postRepository.GetAll();
        }

        public Post Update(Post post)
        {
            var entity = Get(post.Id)
                ?? throw new EntityNotFoundException($"{PostNotFoundMessage}: {post.Id}");

            entity.Title = post.Title;
            entity.Content = post.Content;
            entity.UpdateDate = DateTime.UtcNow;

            return _postRepository.Update(entity);
        }
    }
}
