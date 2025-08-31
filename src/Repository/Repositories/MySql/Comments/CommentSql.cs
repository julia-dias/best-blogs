using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.MySql.Comments
{
    public static class CommentSql
    {
        public const string Insert = @"
            INSERT INTO Comment (Id, PostId, Content, Author, CreationDate, UpdateDate)
            VALUES (@Id, @PostId, @Content, @Author, @CreationDate, @UpdateDate);";

        public const string Update = @"
            UPDATE Comment
            SET Content = @Content,
                Author = @Author,
                UpdateDate = @UpdateDate
            WHERE Id = @Id;";

        public const string Delete = @"
            DELETE FROM Comment
            WHERE Id = @Id;";

        public const string DeleteByPostId = @"
            DELETE FROM Comment
            WHERE PostId = @PostId;";

        public const string GetById = @"
            SELECT Id, PostId, Content, Author, CreationDate, UpdateDate
            FROM Comment
            WHERE Id = @Id;";

        public const string GetAll = @"
            SELECT Id, PostId, Content, Author, CreationDate, UpdateDate
            FROM Comment;";

        public const string GetByPostId = @"
            SELECT Id, PostId, Content, Author, CreationDate, UpdateDate
            FROM Comment
            WHERE PostId = @PostId;";

        public const string GetByPostIdAndCommentId = @"
            SELECT Id, PostId, Content, Author, CreationDate, UpdateDate
            FROM Comment
            WHERE PostId = @PostId AND Id = @Id;";
    }
}
