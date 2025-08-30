using Api.Controllers;
using Api.Dtos.Posts;
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

namespace Api.Tests.Controllers
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
            var expectedDomain = new List<Post>
            {
                new() { Id = Guid.NewGuid(), Title = "First post" },
                new() { Id = Guid.NewGuid(), Title = "Second post" }
            };
            var expectedResponse = new List<PostResponse>
            {
                new() { Id = expectedDomain.First().Id, Title = expectedDomain.First().Title },
                new() { Id = expectedDomain.Last().Id, Title = expectedDomain.Last().Title },
            };
            _mockPostService
                .Setup(s => s.GetAll())
                .Returns(expectedDomain);

            // Act
            var actual = _postController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<PostResponse>>(okObjectResult.Value);

            Assert.Equal(expectedResponse.Count, result.Count());
            Assert.Equal(expectedResponse, result);
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
            var expectedPostResponse = new PostResponse
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
            var result = Assert.IsAssignableFrom<PostResponse>(okObjectResult.Value);

            Assert.Equal(result.Id, expectedId);
            Assert.Equal(result, expectedPostResponse);
        }
    }
}