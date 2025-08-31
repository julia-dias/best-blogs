using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Model.Posts
{
    public interface IPostRepository
    {
        Task<IEnumerable<Post>> GetAllAsync();

        Task<Post> GetAsync(Guid id);

        Task<Post> CreateAsync(Post post);

        Task<Post> UpdateAsync(Post post);

        Task<bool> DeleteAsync(Guid id);
    }
}
