using Api.Controllers.v1;
using Api.Dtos.Comments;
using Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Model.Posts;
using Moq;
using Service.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests.Controllers
{
    public class CommentControllerTests
    {
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly ILogger<CommentController> _logger;
        private readonly CommentController _commentController;

        public CommentControllerTests()
        {
            _mockCommentService = new Mock<ICommentService>();
            _logger = new LoggerFactory().CreateLogger<CommentController>();
            _commentController = new CommentController(_logger, _mockCommentService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOk_AllExisting()
        {
            // Arrange
            var expected = new List<Comment>
            {
                new() { Id = Guid.NewGuid(), PostId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), PostId = Guid.NewGuid() },
            };
            var expectedResponse = new List<CommentResponse>
            {
                new() { Id = expected.First().Id, PostId = expected.First().PostId },
                new() { Id = expected.Last().Id, PostId = expected.Last().PostId },
            };
            _mockCommentService
               .Setup(s => s.GetAllAsync())
               .ReturnsAsync(expected);

            // Act
            var actual = await _commentController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(okObjectResult.Value);

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetById_ReturnsOk_Entity()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedComment = new Comment
            {
                Id = expectedId,
            };
            var expectedCommentResponse = new CommentResponse
            {
                Id = expectedId,
            };

            _mockCommentService
                .Setup(s => s.GetAsync(expectedId))
                .ReturnsAsync(expectedComment);

            // Act
            var actual = await _commentController.Get(expectedId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<CommentResponse>(okObjectResult.Value);

            Assert.Equal(result.Id, expectedId);
            Assert.Equal(result, expectedCommentResponse);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCommentDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockCommentService
                .Setup(s => s.GetAsync(id))
                .ReturnsAsync((Comment)null);

            // Act
            var result = await _commentController.Get(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_Returns201_Entity()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var expectedRequest = new CommentRequest
            {
                PostId = Guid.NewGuid(),
                Content = "some content",
                Author = "any author"
            };
            var expectedDomain = expectedRequest.ToDomain();
            expectedDomain.Id = Guid.NewGuid();
            expectedDomain.CreationDate = now;
            
            _mockCommentService
                .Setup(s => s.CreateAsync(It.IsAny<Comment>()))
                .ReturnsAsync(expectedDomain);

            // Act
            var actionResult = await _commentController.Post(expectedRequest);

            // Assert
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var result = Assert.IsType<CommentResponse>(createdAtAction.Value);

            Assert.Equal(expectedDomain.Id, result.Id);
            Assert.Equal(now, result.CreationDate);
        }

        [Fact]
        public async Task Edit_ReturnsOk_WhenCommentUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CommentRequest { Content = "Updated", Author = "Author" };
            var updatedComment = request.ToDomain(id);
            _mockCommentService
                .Setup(s => s.UpdateAsync(It.IsAny<Comment>()))
                .ReturnsAsync(updatedComment);

            // Act
            var result = await _commentController.Put(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<CommentResponse>(okResult.Value);
            Assert.Equal(updatedComment.Id, response.Id);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new CommentRequest { Content = "Updated", Author = "Author" };
            _mockCommentService
                .Setup(s => s.UpdateAsync(It.IsAny<Model.Comments.Comment>()))
                .ReturnsAsync((Comment)null);

            // Act
            var result = await _commentController.Put(id, request);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockCommentService
                .Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _commentController.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenDeleteFails()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockCommentService
                .Setup(s => s.DeleteAsync(id))
                .ReturnsAsync(false);

            // Act
            var result = await _commentController.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}