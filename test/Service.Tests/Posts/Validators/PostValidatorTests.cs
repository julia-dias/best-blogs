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
        public void ValidatePostExists_ShouldNotThrow_WhenPostExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var post = new Post { Id = id, Title = "Some Post" };
            _postRepositoryMock
                .Setup(r => r.Get(id))
                .Returns(post);

            // Act
            var exception = Record.Exception(() => _validator.ValidatePostExists(id));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void ValidatePostExists_ShouldThrow_WhenPostDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _postRepositoryMock.Setup(r => r.Get(id)).Returns((Post)null);

            // Act & Assert
            var ex = Assert.Throws<EntityNotFoundException>(() => _validator.ValidatePostExists(id));

            Assert.Contains("Post with Id not found", ex.Message);
            Assert.Contains(id.ToString(), ex.Message);
        }
    }
}
