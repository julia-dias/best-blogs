using Dapper;
using Model.Comments;
using Model.Posts;
using Repository.Repositories.MySql.Posts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Common;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace Repository.Repositories.MySql.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IDatabaseConnection _db;

        public CommentRepository(IDatabaseConnection db)
        {
            _db = db;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            var sql = CommentSql.Insert;

            comment.Id = Guid.NewGuid();

            await _db.Connection.ExecuteAsync(sql, comment, _db.Transaction);
            return comment;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sql = CommentSql.Delete;

            var affected = await _db.Connection.ExecuteAsync(sql, new { Id = id }, _db.Transaction);
            return affected > 0;
        }

        public async Task<bool> DeleteByPostIdAsync(Guid postId)
        {
            var sql = CommentSql.DeleteByPostId;

            var affected = await _db.Connection.ExecuteAsync(sql, new { PostId = postId }, _db.Transaction);
            return affected > 0;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            var sql = CommentSql.GetAll;
            return await _db.Connection.QueryAsync<Comment>(sql);
        }

        public async Task<Comment> GetAsync(Guid id)
        {
            var sql = CommentSql.GetById;
            return await _db.Connection.QueryFirstOrDefaultAsync<Comment>(sql, new { Id = id });
        }

        public async Task<Comment> GetByPostIdAndCommentIdAsync(Guid postId, Guid commentId)
        {
            var sql = CommentSql.GetByPostIdAndCommentId;
            return await _db.Connection.QueryFirstOrDefaultAsync<Comment>(sql, new { PostId = postId, Id = commentId });
        }

        public async Task<IEnumerable<Comment>> GetByPostIdAsync(Guid postId)
        {
            var sql = CommentSql.GetByPostId;
            return await _db.Connection.QueryAsync<Comment>(
                sql,
                new { PostId = postId }
            );
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            var sql = CommentSql.Update;
            var updated = await _db.Connection.ExecuteAsync(sql, comment, _db.Transaction);

            if (updated == 0)
            {
                return null;
            }

            return comment;
        }
    }
}
