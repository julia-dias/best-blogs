using Model.Posts;

namespace Service.Posts
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllAsync();

        Task<Post> GetAsync(Guid id);

        Task<Post> CreateAsync(Post post);

        Task<Post> UpdateAsync(Post post);

        Task<bool> DeleteAsync(Guid id);
    }
}
