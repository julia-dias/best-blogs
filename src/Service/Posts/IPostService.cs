using Model.Posts;

namespace Service.Posts
{
    public interface IPostService
    {
        IEnumerable<Post> GetAll();

        Post Get(Guid id);

        Post Create(Post post);

        Post Update(Post post);

        bool Delete(Guid id);
    }
}
