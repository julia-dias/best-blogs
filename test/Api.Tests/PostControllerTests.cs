using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Model.Posts;
using Moq;
using Service.Comments;
using Service.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Api.Tests
{
    public class PostControllerTests
    {
        private readonly Mock<IPostService> _mockPostService;
        private readonly Mock<ICommentService> _mockCommentService;
        private readonly ILogger<PostController> _logger;
        private readonly PostController _postController;

        public PostControllerTests()
        {
            _mockPostService = new Mock<IPostService>();
            _mockCommentService = new Mock<ICommentService>();
            _logger = new LoggerFactory().CreateLogger<PostController>();
            _postController = new PostController(
                _logger,
                _mockPostService.Object,
                _mockCommentService.Object);
        }

        [Fact]
        public void GetAll_Returns_AllExisting()
        {
            // Arrange
            var expected = new List<Post>
            {
                new() { Id = Guid.NewGuid(), Title = "First post" },
                new() { Id = Guid.NewGuid(), Title = "Second post" }
            };
            _mockPostService
                .Setup(s => s.GetAll())
                .Returns(expected);

            // Act
            var actual = _postController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<Post>>(okObjectResult.Value);

            Assert.Equal(expected.Count, result.Count());
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetById_Returns_Entity()
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            var expectedPost = new Post
            {
                Id = expectedId,
            };

            _mockPostService
                .Setup(s => s.Get(expectedId))
                .Returns(expectedPost);

            // Act
            var actual = _postController.Get(expectedId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<Post>(okObjectResult.Value);

            Assert.Equal(result.Id, expectedId);
        }
    }
}