using System.ComponentModel.DataAnnotations;

namespace Api.Dtos.Posts
{
    public record PostRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(30, ErrorMessage = "Title cannot be longer than 30 characters.")]
        public string Title { get; init; }

        [Required(ErrorMessage = "Content is required.")]
        [MaxLength(1200, ErrorMessage = "Content cannot be longer than 1200 characters.")]
        public string Content { get; init; }
    }
}
