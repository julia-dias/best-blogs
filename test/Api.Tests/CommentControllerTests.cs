using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Model.Posts;
using Moq;
using Service.Comments;
using System;
using System.Collections.Generic;
using Xunit;

namespace Api.Tests
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
        public void GetAll_Returns_AllExisting()
        {
            // Arrange
            var expected = new List<Comment>
            {
                new() { Id = Guid.NewGuid(), PostId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), PostId = Guid.NewGuid() },
            };
            _mockCommentService
               .Setup(s => s.GetAll())
               .Returns(expected);

            // Act
            var actual = _commentController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<Comment>>(okObjectResult.Value);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetById_Returns_Entity()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedComment = new Comment
            {
                Id = expectedId,
            };

            _mockCommentService
                .Setup(s => s.Get(expectedId))
                .Returns(expectedComment);

            // Act
            var actual = _commentController.Get(expectedId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<Comment>(okObjectResult.Value);

            Assert.Equal(result.Id, expectedId);
        }
    }
}