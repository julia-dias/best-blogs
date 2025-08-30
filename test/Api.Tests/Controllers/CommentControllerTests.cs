using Api.Controllers;
using Api.Dtos.Comments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Model.Posts;
using Moq;
using Service.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void GetAll_Returns_AllExisting()
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
               .Setup(s => s.GetAll())
               .Returns(expected);

            // Act
            var actual = _commentController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(okObjectResult.Value);

            Assert.Equal(expectedResponse, result);
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
            var expectedCommentResponse = new CommentResponse
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
            var result = Assert.IsAssignableFrom<CommentResponse>(okObjectResult.Value);

            Assert.Equal(result.Id, expectedId);
            Assert.Equal(result, expectedCommentResponse);
        }

        //[Fact]
        //public void GetById_Returns_NotFound()
        //{

        //}

        [Fact]
        public void Create_Returns_Entity()
        {
            // Arrange
            var expectedRequest = new CommentRequest
            {
                PostId = Guid.NewGuid(),
                Content = "some content",
                Author = "any author"
            };

            var expectedDomain = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = expectedRequest.PostId,
                Content = expectedRequest.Content,
                Author = expectedRequest.Author,
                CreationDate = DateTime.UtcNow,
            };

            _mockCommentService
                .Setup(s => s.Create(It.IsAny<Comment>()))
                .Returns(expectedDomain);

            // Act
            var actionResult = _commentController.Post(expectedRequest);

            // Assert
            var createdAtAction = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var result = Assert.IsType<CommentResponse>(createdAtAction.Value);

            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(expectedRequest.PostId, result.PostId);
            Assert.Equal(expectedRequest.Content, result.Content);
            Assert.Equal(expectedRequest.Author, result.Author);
        }
    }
}