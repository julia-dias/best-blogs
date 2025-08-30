using Model.Comments;
using Model.Posts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _context;

        public PostRepository(BlogContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.ToList();
        }

        public Post Get(Guid id)
        {
            return _context.Posts.Find(id);
        }

        public Post Create(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return post;
        }

        public Post Update(Post post)
        {
            _context.Posts.Update(post);
            _context.SaveChanges();
            return post;
        }

        public bool Delete(Guid id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return false;
            }

            _context.Posts.Remove(post);
            _context.SaveChanges();
            return true;
        }
    }
}