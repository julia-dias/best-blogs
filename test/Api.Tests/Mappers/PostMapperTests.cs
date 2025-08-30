using Api.Dtos.Posts;
using Api.Mappers;
using Model.Posts;
using System;
using Xunit;

namespace Api.Tests.Mappers
{
    public class PostMapperTests
    {
        [Fact]
        public void ToResponse_ShouldMapDomainToResponseCorrectly()
        {
            // Arrange
            var domain = new Post
            {
                Id = Guid.NewGuid(),
                Title = "Test Post",
                Content = "This is a test post",
                CreationDate = DateTime.UtcNow
            };

            // Act
            var response = domain.ToResponse();

            // Assert
            Assert.Equal(domain.Id, response.Id);
            Assert.Equal(domain.Title, response.Title);
            Assert.Equal(domain.Content, response.Content);
            Assert.Equal(domain.CreationDate, response.CreationDate);
        }

        [Fact]
        public void ToDomain_ShouldMapRequestToDomainCorrectly()
        {
            // Arrange
            var request = new PostRequest
            {
                Title = "Test Post",
                Content = "This is a test post"
            };

            // Act
            var domain = request.ToDomain();

            // Assert
            Assert.Equal(request.Title, domain.Title);
            Assert.Equal(request.Content, domain.Content);
            Assert.True((DateTime.UtcNow - domain.CreationDate).TotalSeconds < 1,
                        "CreationDate should be set to now");
        }
    }
}
