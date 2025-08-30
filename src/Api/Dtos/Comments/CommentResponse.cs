using System;

namespace Api.Dtos.Comments
{
    public record CommentResponse
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
