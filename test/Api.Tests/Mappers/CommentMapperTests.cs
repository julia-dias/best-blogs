using Api.Dtos.Comments;
using Api.Mappers;
using Model.Comments;
using System;
using Xunit;

namespace Api.Tests.Mappers
{
    public class CommentMapperTests
    {
        [Fact]
        public void ToResponse_ShouldMapDomainToResponseCorrectly()
        {
            // Arrange
            var domain = new Comment
            {
                Id = Guid.NewGuid(),
                PostId = Guid.NewGuid(),
                Content = "Test comment",
                Author = "John Doe",
                CreationDate = DateTime.UtcNow
            };

            // Act
            var response = domain.ToResponse();

            // Assert
            Assert.Equal(domain.Id, response.Id);
            Assert.Equal(domain.PostId, response.PostId);
            Assert.Equal(domain.Content, response.Content);
            Assert.Equal(domain.Author, response.Author);
            Assert.Equal(domain.CreationDate, response.CreationDate);
        }

        [Fact]
        public void ToDomain_ShouldMapRequestToDomainCorrectly()
        {
            // Arrange
            var request = new CommentRequest
            {
                PostId = Guid.NewGuid(),
                Content = "Test comment",
                Author = "John Doe"
            };

            // Act
            var domain = request.ToDomain();

            // Assert
            Assert.Equal(request.PostId, domain.PostId);
            Assert.Equal(request.Content, domain.Content);
            Assert.Equal(request.Author, domain.Author);
            Assert.True((DateTime.UtcNow - domain.CreationDate).TotalSeconds < 1, "CreationDate should be set to now");
        }
    }
}
