using Api.Dtos.Posts;
using Model.Posts;
using System;

namespace Api.Mappers
{
    public static class PostMapper
    {
        public static PostResponse ToResponse(this Post domain)
        {
            return new PostResponse
            {
                Id = domain.Id,
                Title = domain.Title,
                Content = domain.Content,
                CreationDate = domain.CreationDate,
                UpdateDate = domain.UpdateDate,
            };
        }

        public static Post ToDomain(this PostRequest request)
        {
            return new Post
            {
                Title = request.Title,
                Content = request.Content
            };
        }

        public static Post ToDomain(this PostRequest request, Guid id)
        {
            return new Post
            {
                Id = id,
                Title = request.Title,
                Content = request.Content
            };
        }
    }
}
