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
        public void Create_ShouldSetCreationDate_AndCallRepository()
        {
            // Arrange
            var post = new Post { Title = "Test" };
            var created = new Post { Id = Guid.NewGuid(), Title = "Test" };

            _postRepositoryMock
                .Setup(r => r.Create(It.IsAny<Post>()))
                .Returns(created);

            // Act
            var result = _service.Create(post);

            // Assert
            _postRepositoryMock.Verify(r => r.Create(It.Is<Post>(p =>
                p.CreationDate != default)), Times.Once);

            Assert.Equal(created.Id, result.Id);
        }

        [Fact]
        public void Delete_ShouldDeleteComments_ThenDeletePost()
        {
            // Arrange
            var id = Guid.NewGuid();
            _commentServiceMock
                .Setup(c => c.DeleteByPostId(id))
                .Returns(true);
            _postRepositoryMock
                .Setup(r => r.Delete(id))
                .Returns(true);

            // Act
            var result = _service.Delete(id);

            // Assert
            Assert.True(result);
            _commentServiceMock.Verify(c => c.DeleteByPostId(id), Times.Once);
            _postRepositoryMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public void Delete_ShouldStillDeletePost_WhenNoCommentsDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            _commentServiceMock
                .Setup(c => c.DeleteByPostId(id))
                .Returns(false);
            _postRepositoryMock
                .Setup(r => r.Delete(id))
                .Returns(true);

            // Act
            var result = _service.Delete(id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Get_ShouldReturnPost_FromRepository()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid(), Title = "Test" };
            _postRepositoryMock
                .Setup(r => r.Get(post.Id))
                .Returns(post);

            // Act
            var result = _service.Get(post.Id);

            // Assert
            Assert.Equal(post, result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllPosts_FromRepository()
        {
            // Arrange
            var posts = new List<Post> { new Post { Id = Guid.NewGuid() } };
            _postRepositoryMock
                .Setup(r => r.GetAll())
                .Returns(posts);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(posts, result);
        }

        [Fact]
        public void Update_ShouldUpdatePost_WhenExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var post = new Post { Id = id, Title = "Updated", Content = "New" };
            var existing = new Post { Id = id, Title = "Old", Content = "Old" };

            _postRepositoryMock
                .Setup(r => r.Get(id))
                .Returns(existing);
            _postRepositoryMock
                .Setup(r => r.Update(It.IsAny<Post>()))
                .Returns((Post p) => p);

            // Act
            var result = _service.Update(post);

            // Assert
            Assert.Equal(post.Title, result.Title);
            Assert.Equal(post.Content, result.Content);
            Assert.NotEqual(default, result.UpdateDate);
        }

        [Fact]
        public void Update_ShouldThrow_WhenPostDoesNotExist()
        {
            // Arrange
            var post = new Post { Id = Guid.NewGuid(), Title = "X" };
            _postRepositoryMock
                .Setup(r => r.Get(post.Id))
                .Returns((Post)null);

            // Act & Assert
            Assert.Throws<EntityNotFoundException>(() => _service.Update(post));
        }
    }
}
