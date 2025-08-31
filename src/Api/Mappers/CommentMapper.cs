using Api.Dtos.Comments;
using Api.Dtos.Posts;
using Model.Comments;
using Model.Posts;
using System;

namespace Api.Mappers
{
    public static class CommentMapper
    {
        public static CommentResponse ToResponse(this Comment domain)
        {
            return new CommentResponse
            {
                Id = domain.Id,
                PostId = domain.PostId,
                Content = domain.Content,
                Author = domain.Author,
                CreationDate = domain.CreationDate,
                UpdateDate = domain.UpdateDate,
            };
        }

        public static Comment ToDomain(this CommentRequest request)
        {
            return new Comment
            {
                PostId = request.PostId,
                Content = request.Content,
                Author = request.Author
            };
        }

        public static Comment ToDomain(this CommentRequest request, Guid id)
        {
            return new Comment
            {
                Id = id,
                PostId = request.PostId,
                Content = request.Content,
                Author = request.Author
            };
        }
    }
}
