using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Comments;
using Model.Posts;
using Repository.Repositories;
using Repository.Repositories.MySql;
using Repository.Repositories.MySql.Comments;
using Repository.Repositories.MySql.Posts;
using Service.Comments;
using Service.Posts;
using Service.Posts.Validators;
using System;
using InMemory = Repository.Repositories.InMemory;

namespace Api.Extensions
{
    internal static class DependenciesExtensions
    {
        internal static void RegisterRepositories(
            this IServiceCollection services,
            bool useInMemoryDb)
        {
            if (useInMemoryDb)
            {
                services.AddScoped<ICommentRepository, InMemory.CommentRepository>();
                services.AddScoped<IPostRepository, InMemory.PostRepository>();
            }
            else
            {
                services.AddScoped<ICommentRepository, CommentRepository>();
                services.AddScoped<IPostRepository, PostRepository>();
            }
        }

        internal static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IPostValidator, PostValidator>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
        }

        internal static void RegisterDatabase(
            this IServiceCollection services,
            IConfiguration Configuration,
            bool useInMemoryDb)
        {
            if (useInMemoryDb)
            {
                services.AddDbContext<InMemory.BlogContext>(x => x.UseInMemoryDatabase("InMemoryDb"));
            }
            else
            {
                var connectionString = Configuration.GetConnectionString("DB_BESTBLOGS");

                services.AddScoped<IDatabaseConnection>((provider) =>
                {
                    return new DatabaseConnection(connectionString);
                });

            }
        }
    }
}
