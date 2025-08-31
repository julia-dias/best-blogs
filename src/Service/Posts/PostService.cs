using Model.Posts;
using Service.Exceptions;

namespace Service.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        private const string PostNotFoundMessage = "Post with Id not found";

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Post Create(Post post)
        {
            post.CreationDate = DateTime.UtcNow;

            return _postRepository.Create(post);
        }

        public bool Delete(Guid id)
        {
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
