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
        public void Create_Should_CallRepository_When_PostExists()
        {
            // Arrange
            var comment = new Comment 
            { 
                PostId = Guid.NewGuid(),
                Content = "Test",
                Author = "Author"
            };
            _mockPostValidator
                .Setup(p => p.ValidatePostExists(comment.PostId));
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
            _mockPostValidator
               .Setup(p => p.ValidatePostExists(comment.PostId))
               .Throws(new EntityNotFoundException($"{PostNotFoundMessage}: {comment.PostId}")); ;

            // Act & Assert
            var ex = Assert.Throws<EntityNotFoundException>(() => _commentService.Create(comment));

            Assert.Equal($"{PostNotFoundMessage}: {comment.PostId}", ex.Message);
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
                Author = "Author",
                CreationDate = DateTime.UtcNow,
            };
            _mockCommentRepository
                .Setup(p => p.GetByPostIdAndCommentId(comment.PostId, comment.Id))
                .Returns(comment);

            _mockCommentRepository
                .Setup(r => r.Update(comment))
                .Returns(comment);

            // Act
            var result = _commentService.Update(comment);

            // Assert
            Assert.Equal(comment.Id, result.Id);
            Assert.Equal(comment.PostId, result.PostId);
            Assert.Equal(comment.Content, result.Content);
            Assert.Equal(comment.Author, result.Author);
            Assert.Equal(comment.CreationDate, result.CreationDate);
            Assert.NotNull(result.UpdateDate);
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

            _mockCommentRepository
                .Setup(p => p.GetByPostIdAndCommentId(comment.PostId, comment.Id))
                .Returns((Comment)null);

            // Act & Assert
            var ex = Assert.Throws<EntityNotFoundException>(() => _commentService.Update(comment));
            Assert.Equal($"{CommentNotFoundInPostMessage}: {comment.PostId}", ex.Message);
            _mockCommentRepository.Verify(r => r.Update(It.IsAny<Comment>()), Times.Never);
        }
    }
}