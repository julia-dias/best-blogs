using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model.Comments;

namespace Repository.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogContext _context;

        public CommentRepository(BlogContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.ToList();
        }

        public Comment Get(Guid id)
        {
            return _context.Comments.Find(id);
        }

        public Comment Create(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
            return comment;
        }

        public bool Delete(Guid id)
        {
            var comment = _context.Comments.Find(id);
            if (comment == null)
            {
                return false;
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteByPostId(Guid postId)
        {
            var comments = _context.Comments
                .Where(c => c.PostId == postId)
                .ToList();

            if (!comments.Any())
            {
                return false;
            }

            _context.Comments.RemoveRange(comments);
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Comment> GetByPostId(Guid postId)
        {
            return _context.Comments
                .Where(c => c.PostId == postId);
        }

        public Comment GetByPostIdAndCommentId(Guid postId, Guid commentId)
        {
            return _context.Comments
                .FirstOrDefault(c => c.Id == commentId && c.PostId == postId);
        }
    }
}