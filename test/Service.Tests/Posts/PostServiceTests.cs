using Microsoft.Extensions.Logging;
using Model.Posts;
using Moq;
using Service.Comments;
using Service.Exceptions;
using Service.Posts;

namespace Service.Tests.Posts
{
    public class PostServiceTests
    {
        private readonly Mock<ILogger<PostService>> _loggerMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<ICommentService> _commentServiceMock;
        private readonly PostService _service;

        public PostServiceTests()
        {
            _loggerMock = new Mock<ILogger<PostService>>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _commentServiceMock = new Mock<ICommentService>();

            _service = new PostService(
                _loggerMock.Object,
                _postRepositoryMock.Object,
                _commentServiceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldSetCreationDate_AndCallRepository()
        {
            // Arrange
            var post = new Post { Title = "Test" };
            var created = new Post { Id = Guid.NewGuid(), Title = "Test" };

            _postRepositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Post>()))
                .ReturnsAsync(created);

            // Act
            var result = await _service.CreateAsync(post);

            // Assert
            _postRepositoryMock.Verify(r => r.CreateAsync(It.Is<Post>(p =>
                p.CreationDate != default)), Times.Once);

            Assert.Equal(created.Id, result.Id);
        }

        [Fact]
        public async Task Delete_ShouldDeleteComments_ThenDeletePost()
        {
            // Arrange
            var id = Guid.NewGuid();
            _commentServiceMock
                .Setup(c => c.DeleteByPostIdAsync(id))
                .ReturnsAsync(true);
            _postRepositoryMock
                .Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.True(result);
            _commentServiceMock.Verify(c => c.DeleteByPostIdAsync(id), Times.Once);
            _postRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldStillDeletePost_WhenNoCommentsDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            _commentServiceMock
                .Setup(c => c.DeleteByPostIdAsync(id))
                .ReturnsAsync(false);
            _postRepositoryMock
                .Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteAsync(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Get_ShouldReturnPost_FromRepository()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid(), Title = "Test" };
            _postRepositoryMock
                .Setup(r => r.GetAsync(post.Id))
                .ReturnsAsync(post);

            // Act
            var result = await _service.GetAsync(post.Id);

            // Assert
            Assert.Equal(post, result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllPosts_FromRepository()
        {
            // Arrange
            var posts = new List<Post> { new Post { Id = Guid.NewGuid() } };
            _postRepositoryMock
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(posts);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(posts, result);
        }

        [Fact]
        public async Task Update_ShouldUpdatePost_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var post = new Post { Id = id, Title = "Updated", Content = "New" };
            var existing = new Post { Id = id, Title = "Old", Content = "Old" };

            _postRepositoryMock
                .Setup(r => r.GetAsync(id))
                .ReturnsAsync(existing);
            _postRepositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Post>()))
                .ReturnsAsync((Post p) => p);

            // Act
            var result = await _service.UpdateAsync(post);

            // Assert
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.Content, result.Content);
            Assert.NotEqual(default, result.UpdateDate);
        }

        [Fact]
        public async Task Update_ShouldThrow_WhenPostDoesNotExist()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid(), Title = "X" };
            _postRepositoryMock
                .Setup(r => r.GetAsync(post.Id))
                .ReturnsAsync((Post)null);

            // Act & Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(() => _service.UpdateAsync(post));
        }
    }
}
