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
using System.Threading.Tasks;
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
        public async Task GetAll_ReturnsOk_AllExisting()
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
                .Setup(s => s.GetAllAsync())
                .ReturnsAsync(expectedDomain);

            // Act
            var actual = await _postController.GetAll();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<IEnumerable<PostResponse>>(okObjectResult.Value);

            Assert.Equal(expectedResponse.Count, result.Count());
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetById_ReturnsOk_Entity()
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
                .Setup(s => s.GetAsync(expectedId))
                .ReturnsAsync(expectedPost);

            // Act
            var actual = await _postController.Get(expectedId);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(actual.Result);
            var result = Assert.IsAssignableFrom<PostResponse>(okObjectResult.Value);

            Assert.Equal(result.Id, expectedId);
            Assert.Equal(result, expectedPostResponse);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenPostDoesNotExist()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Post)null);

            // Act
            var result = await _postController.Get(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Create_Returns201_WithPostResponse()
        {
            // Arrange
            var request = new PostRequest { Title = "New Post" };
            var created = new Post { Id = Guid.NewGuid(), Title = request.Title };

            _mockPostService
                .Setup(x => x.CreateAsync(It.IsAny<Post>()))
                .ReturnsAsync(created);

            // Act
            var result = await _postController.Post(request);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(PostController.Get), createdAt.ActionName);
            var response = Assert.IsType<PostResponse>(createdAt.Value);
            Assert.Equal(created.Id, response.Id);
        }

        [Fact]
        public async Task Edit_ReturnsOk_WhenPostUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new PostRequest { Title = "Updated" };
            var updated = new Post { Id = id, Title = "Updated" };

            _mockPostService
                .Setup(x => x.UpdateAsync(It.IsAny<Post>()))
                .ReturnsAsync(updated);

            // Act
            var result = await _postController.Put(id, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<PostResponse>(okResult.Value);
            Assert.Equal(updated.Id, response.Id);
        }

        [Fact]
        public async Task Edit_ReturnNotFound_WhenUpdateFails()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.UpdateAsync(It.IsAny<Post>()))
                .ReturnsAsync((Post)null);

            // Act
            var result = await _postController.Put(Guid.NewGuid(), new PostRequest());

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenDeleted()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _postController.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenNotDeleted()
        {
            // Arrange
            _mockPostService
                .Setup(x => x.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _postController.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCommentsByPostId_ShouldReturnOk_WithComments()
        {
            // Arrange
            var postId = Guid.NewGuid();
            var comments = new List<Comment>
            {
                new() { Id = Guid.NewGuid(), PostId = postId, Content = "Hello" }
            };
            _mockCommentService
                .Setup(x => x.GetByPostIdAsync(postId))
                .ReturnsAsync(comments);

            // Act
            var result = await _postController.GetComments(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(okResult.Value);
            Assert.Single(response);
        }

        [Fact]
        public async Task GetCommentsByPostId_ShouldReturnOk_AndEmpty()
        {
            // Arrange
            var postId = Guid.NewGuid();
            _mockCommentService
                .Setup(x => x.GetByPostIdAsync(postId))
                .ReturnsAsync(new List<Comment>());

            // Act
            var result = await _postController.GetComments(postId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsAssignableFrom<IEnumerable<CommentResponse>>(okResult.Value);
            Assert.True(response.Count() == 0);
        }
    }
}