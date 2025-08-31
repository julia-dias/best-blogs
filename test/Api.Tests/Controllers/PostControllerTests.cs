using Api.Controllers.v1;
using Api.Dtos.Comments;
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
        public void GetAll_ReturnsOk_AllExisting()
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
        public void GetById_ReturnsOk_Entity()
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

        [Fact]
        public void GetById_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.Get(It.IsAny<Guid>()))
                .Returns((Post)null);

            // Act
            var result = _postController.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Create_Returns201_WithPostResponse()
        {
            // Arrange
            var request = new PostRequest { Title = "New Post" };
            var created = new Post { Id = Guid.NewGuid(), Title = request.Title };

            _mockPostService
                .Setup(x => x.Create(It.IsAny<Post>()))
                .Returns(created);

            // Act
            var result = _postController.Post(request);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(PostController.Get), createdAt.ActionName);
            var response = Assert.IsType<PostResponse>(createdAt.Value);
            Assert.Equal(created.Id, response.Id);
        }

        [Fact]
        public void Edit_ReturnsOk_WhenPostUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PostRequest { Title = "Updated" };
            var updated = new Post { Id = id, Title = "Updated" };

            _mockPostService
                .Setup(x => x.Update(It.IsAny<Post>()))
                .Returns(updated);

            // Act
            var result = _postController.Put(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PostResponse>(okResult.Value);
            Assert.Equal(updated.Id, response.Id);
        }

        [Fact]
        public void Edit_ReturnNotFound_WhenUpdateFails()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.Update(It.IsAny<Post>()))
                .Returns((Post)null);

            // Act
            var result = _postController.Put(Guid.NewGuid(), new PostRequest());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent_WhenDeleted()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns(true);

            // Act
            var result = _postController.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenNotDeleted()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.Delete(It.IsAny<Guid>()))
                .Returns(false);

            // Act
            var result = _postController.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetCommentsByPostId_ShouldReturnOk_WithComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new() { Id = Guid.NewGuid(), PostId = postId, Content = "Hello" }
            };
            _mockCommentService
                .Setup(x => x.GetByPostId(postId))
                .Returns(comments);

            // Act
            var result = _postController.GetComments(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(okResult.Value);
            Assert.Single(response);
        }

        [Fact]
        public void GetCommentsByPostId_ShouldReturnOk_AndEmpty()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _mockCommentService
                .Setup(x => x.GetByPostId(postId))
                .Returns(new List<Comment>());

            // Act
            var result = _postController.GetComments(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(okResult.Value);
            Assert.True(response.Count() == 0);
        }
    }
}