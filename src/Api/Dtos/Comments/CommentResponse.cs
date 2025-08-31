using System;

namespace Api.Dtos.Comments
{
    public record CommentResponse
    {
        public Guid Id { get; init; }

        public Guid PostId { get; init; }

        public string Content { get; init; }

        public string Author { get; init; }

        public DateTime CreationDate { get; init; }

        public DateTime? UpdateDate { get; init; }
    }
}
