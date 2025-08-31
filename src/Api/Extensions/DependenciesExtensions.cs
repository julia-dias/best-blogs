using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Model.Comments;
using Model.Posts;
using Repository.Repositories;
using Repository.Repositories.InMemory;
using Service.Comments;
using Service.Posts;
using Service.Posts.Validators;
using System.Configuration;

namespace Api.Extensions
{
    internal static class DependenciesExtensions
    {
        internal static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
        }

        internal static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IPostValidator, PostValidator>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
        }

        internal static void RegisterDatabase(
            this IServiceCollection services,
            IConfiguration Configuration)
        {
            var useInMemoryDb = Configuration.GetValue<bool>("USE_IN_MEMORY_DB");
            if (useInMemoryDb)
            {
                services.AddDbContext<BlogContext>(x => x.UseInMemoryDatabase("InMemoryDb"));
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
