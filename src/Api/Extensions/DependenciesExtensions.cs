using Microsoft.Extensions.DependencyInjection;
using Model.Comments;
using Model.Posts;
using Repository.Repositories;
using Service.Comments;
using Service.Posts;
using Service.Posts.Validators;

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
    }
}
