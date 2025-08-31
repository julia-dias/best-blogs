using Model.Comments;
using Model.Posts;
using Moq;
using Service.Comments;
using Service.Exceptions;
using Service.Posts;
using Service.Posts.Validators;

namespace Service.Tests.Comments
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IPostValidator> _mockPostValidator;
        private readonly CommentService _commentService;

        private const string PostNotFoundMessage = "Post with Id not found";
        private const string CommentNotFoundInPostMessage = "Comment not found in Post";

        public CommentServiceTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockPostValidator = new Mock<IPostValidator>();
            _commentService = new CommentService(
                _mockCommentRepository.Object,
                _mockPostValidator.Object);
        }

        [Fact]
        public async Task Create_Should_CallRepository_When_PostExists()
        {
            // Arrange
            var comment = new Comment 
            { 
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author"
            };
            _mockPostValidator
                .Setup(p => p.ValidatePostExistsAsync(comment.PostId));
            _mockCommentRepository
                .Setup(r => r.CreateAsync(comment))
                .ReturnsAsync(comment);

            // Act
            var result = await _commentService.CreateAsync(comment);

            // Assert
            Assert.Equal(comment, result);
            _mockCommentRepository.Verify(r => r.CreateAsync(comment), Times.Once);
        }

        [Fact]
        public async Task Create_Should_Throw_When_PostDoesNotExist()
        {
            // Arrange
            var comment = new Comment
            {
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author"
            };
            _mockPostValidator
               .Setup(p => p.ValidatePostExistsAsync(comment.PostId))
               .ThrowsAsync(new EntityNotFoundException($"{PostNotFoundMessage}: {comment.PostId}")); ;

            // Act & Assert
            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _commentService.CreateAsync(comment));

            Assert.Equal($"{PostNotFoundMessage}: {comment.PostId}", ex.Message);
            _mockCommentRepository.Verify(r => r.CreateAsync(It.IsAny<Comment>()), Times.Never);
        }

        [Fact]
        public async Task Update_Should_CallRepository_When_PostExists()
        {
            // Arrange
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author",
                CreationDate = DateTime.UtcNow,
            };
            _mockCommentRepository
                .Setup(p => p.GetByPostIdAndCommentIdAsync(comment.PostId, comment.Id))
                .ReturnsAsync(comment);

            _mockCommentRepository
                .Setup(r => r.UpdateAsync(comment))
                .ReturnsAsync(comment);

            // Act
            var result = await _commentService.UpdateAsync(comment);

            // Assert
            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.PostId, result.PostId);
            Assert.Equal(comment.Content, result.Content);
            Assert.Equal(comment.Author, result.Author);
            Assert.Equal(comment.CreationDate, result.CreationDate);
            Assert.NotNull(result.UpdateDate);
            _mockCommentRepository.Verify(r => r.UpdateAsync(comment), Times.Once);
        }

        [Fact]
        public async Task Update_Should_Throw_When_PostDoesNotExist()
        {
            // Arrange
            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author" };

            _mockCommentRepository
                .Setup(p => p.GetByPostIdAndCommentIdAsync(comment.PostId, comment.Id))
                .ReturnsAsync((Comment)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<EntityNotFoundException>(() => _commentService.UpdateAsync(comment));
            Assert.Equal($"{CommentNotFoundInPostMessage}: {comment.PostId}", ex.Message);
            _mockCommentRepository.Verify(r => r.UpdateAsync(It.IsAny<Comment>()), Times.Never);
        }
    }
}