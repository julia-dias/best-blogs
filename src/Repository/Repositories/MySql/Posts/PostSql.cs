namespace Repository.Repositories.MySql.Posts
{
    public static class PostSql
    {
        public const string GetAll = @"
            SELECT Id, Title, Content, CreationDate, UpdateDate
            FROM Post";

        public const string GetById = @"
            SELECT Id, Title, Content, CreationDate, UpdateDate
            FROM Post
            WHERE Id = @Id";

        public const string Insert = @"
            INSERT INTO Post (Id, Title, Content, CreationDate) 
            VALUES (@Id, @Title, @Content, @CreationDate)";

        public const string Update = @"
            UPDATE Post
            SET Title = @Title,
                Content = @Content,
                UpdateDate = @UpdateDate
            WHERE Id = @Id";

        public const string Delete = @"
            DELETE FROM Post 
            WHERE Id = @Id";
    }
}
