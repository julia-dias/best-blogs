using Model.Posts;

namespace Service.Posts
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Post Create(Post post)
        {
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
            return _postRepository.Update(post);
        }
    }
}
