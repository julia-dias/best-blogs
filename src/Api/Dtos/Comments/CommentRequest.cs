using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Comments
{
    public record CommentRequest
    {
        [Required(ErrorMessage = "PostId is required.")]
        public Guid PostId { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [MaxLength(120, ErrorMessage = "Content cannot be longer than 120 characters.")]
        public string Content { get; init; }

        [Required(ErrorMessage = "Author is required.")]
        [MaxLength(30, ErrorMessage = "Author cannot be longer than 30 characters.")]
        public string Author { get; init; }
    }
}
