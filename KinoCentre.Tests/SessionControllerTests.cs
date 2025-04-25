// csharp
using Xunit;
using Moq;
using KinoCentre.Controllers;
using KinoCentre.DB.Entities;
using KinoCentre.Models;
using Microsoft.AspNetCore.Mvc;

namespace KinoCentre.Tests
{
    public class SessionControllerTests
    {
        [Fact]
        public async Task GetAllPaginatedSessions_ReturnsOk_WithSessions()
        {
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.SessionsRepository.GetSessionsWithMovie())
                .ReturnsAsync(new List<Repository<Session>.SessionWithMovie>());

            var controller = new SessionController(mockUow.Object);

            var result = await controller.GetAllPaginatedSessions() as OkObjectResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AddSession_ReturnsOk_WithSession()
        {
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.SessionsRepository.InsertAsync(It.IsAny<Session>()))
                .Returns(Task.CompletedTask);
            var controller = new SessionController(mockUow.Object);

            var model = new SessionAddModel
            {
                MovieId = "M1",
                SessionDateTime = System.DateTime.Now
            };

            var result = await controller.AddSession(model) as OkObjectResult;

            Assert.NotNull(result);
            var session = result.Value as Session;
            Assert.Equal("M1", session.MovieId);
        }
    }
}