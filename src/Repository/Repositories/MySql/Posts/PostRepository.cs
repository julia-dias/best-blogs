using Dapper;
using Model.Posts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories.MySql.Posts
{
    public class PostRepository : IPostRepository
    {
        private readonly IDatabaseConnection _db;

        public PostRepository(IDatabaseConnection db)
        {
            _db = db;
        }

        public async Task<Post> CreateAsync(Post post)
        {
            var sql = PostSql.Insert;

            post.Id = Guid.NewGuid();
            await _db.Connection.ExecuteAsync(sql, post, _db.Transaction);
            return post;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var sql = PostSql.Delete;

            var affected = await _db.Connection.ExecuteAsync(sql, new { Id = id }, _db.Transaction);
            return affected > 0;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            var sql = PostSql.GetAll;
            return await _db.Connection.QueryAsync<Post>(sql);
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var sql = PostSql.GetById;
            return await _db.Connection.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
        }

        public async Task<Post> UpdateAsync(Post post)
        {
            var sql = PostSql.Update;
            var updated = await _db.Connection.ExecuteAsync(sql, post, _db.Transaction);

            if (updated == 0)
            {
                return null;
            }

            return post;
        }
    }
}
