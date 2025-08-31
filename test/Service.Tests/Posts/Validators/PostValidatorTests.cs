using Model.Posts;
using Moq;
using Service.Exceptions;
using Service.Posts.Validators;

namespace Service.Tests.Posts.Validators
{
    public class PostValidatorTests
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly PostValidator _validator;

        public PostValidatorTests()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _validator = new PostValidator(_postRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidatePostExists_ShouldNotThrow_WhenPostExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var post = new Post { Id = id, Title = "Some Post" };
            _postRepositoryMock
                .Setup(r => r.GetAsync(id))
                .ReturnsAsync(post);

            // Act
            var exception = await Record.ExceptionAsync(() => _validator.ValidatePostExistsAsync(id));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task ValidatePostExists_ShouldThrow_WhenPostDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _postRepositoryMock
                .Setup(r => r.GetAsync(id))
                .ReturnsAsync((Post)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _validator.ValidatePostExistsAsync(id));

            Assert.Contains("Post with Id not found", ex.Message);
            Assert.Contains(id.ToString(), ex.Message);
        }
    }
}
