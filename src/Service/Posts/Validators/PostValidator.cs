using Model.Posts;
using Service.Exceptions;

namespace Service.Posts.Validators
{
    public class PostValidator : IPostValidator
    {
        private readonly IPostRepository _postRepository;

        private const string PostNotFoundMessage = "Post with Id not found";

        public PostValidator(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task ValidatePostExistsAsync(Guid postId)
        {
            if (await _postRepository.GetAsync(postId) is null)
            {
                throw new EntityNotFoundException($"{PostNotFoundMessage}: {postId}");
            }
        }
    }
}
