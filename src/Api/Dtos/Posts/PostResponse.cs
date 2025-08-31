using System;

namespace Api.Dtos.Posts
{
    public record PostResponse
    {
        public Guid Id { get; init; }

        public string Title { get; init; }

        public string Content { get; init; }

        public DateTime CreationDate { get; init; }

        public DateTime? UpdateDate { get; init; }
    }
}
