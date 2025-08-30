namespace Api.Dtos.Posts
{
    public record PostRequest
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
