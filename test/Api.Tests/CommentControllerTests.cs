using System.Collections.Generic;
using Api.Controllers;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Moq;
using Service.Comments;
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
        public void GetAll_Returns_Existing_Comments()
        {
            // Arrange
            var expected = new List<Comment>();

            // Act
            var actual = _commentController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            Assert.Equal(expected, okObjectResult.Value);
        }
    }
}