using Model.Comments;
using Model.Posts;
using Moq;
using Service.Comments;
using Service.Posts;

namespace Service.Tests.Comments
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IPostService> _mockPostService;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockPostService = new Mock<IPostService>();
            _commentService = new CommentService(
                _mockCommentRepository.Object,
                _mockPostService.Object);
        }

        [Fact]
        public void Create_Should_CallRepository_When_PostExists()
        {
            // Arrange
            var comment = new Comment 
            { 
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author"
            };
            _mockPostService
                .Setup(p => p.Get(comment.PostId))
                .Returns(new Post { Id = comment.PostId });
            _mockCommentRepository
                .Setup(r => r.Create(comment)).Returns(comment);

            // Act
            var result = _commentService.Create(comment);

            // Assert
            Assert.Equal(comment, result);
            _mockCommentRepository.Verify(r => r.Create(comment), Times.Once);
        }

        [Fact]
        public void Create_Should_Throw_When_PostDoesNotExist()
        {
            // Arrange
            var comment = new Comment
            {
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author"
            };
            _mockPostService
                .Setup(p => p.Get(comment.PostId))
                .Returns((Post)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _commentService.Create(comment));
            Assert.Equal("PostId not found.", ex.Message);
            _mockCommentRepository.Verify(r => r.Create(It.IsAny<Comment>()), Times.Never);
        }

        [Fact]
        public void Update_Should_CallRepository_When_PostExists()
        {
            // Arrange
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author"
            };
            _mockPostService
                .Setup(p => p.Get(comment.PostId))
                .Returns(new Post { Id = comment.PostId });
            _mockCommentRepository
                .Setup(r => r.Update(comment)).Returns(comment);

            // Act
            var result = _commentService.Update(comment);

            // Assert
            Assert.Equal(comment, result);
            _mockCommentRepository.Verify(r => r.Update(comment), Times.Once);
        }

        [Fact]
        public void Update_Should_Throw_When_PostDoesNotExist()
        {
            // Arrange
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author" };
            _mockPostService
                .Setup(p => p.Get(comment.PostId)).Returns((Post)null);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _commentService.Update(comment));
            Assert.Equal("PostId not found.", ex.Message);
            _mockCommentRepository.Verify(r => r.Update(It.IsAny<Comment>()), Times.Never);
        }
    }
}