using System;

namespace Api.Dtos.Comments
{
    public record CommentRequest
    {
        public Guid PostId { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }
    }
}
