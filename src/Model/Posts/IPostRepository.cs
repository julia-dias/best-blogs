using System;
using System.Collections.Generic;

namespace Model.Posts
{
    public interface IPostRepository
    {
        IEnumerable<Post> GetAll();

        Post Get(Guid id);

        Post Create(Post post);

        Post Update(Post post);

        bool Delete(Guid id);
    }
}
