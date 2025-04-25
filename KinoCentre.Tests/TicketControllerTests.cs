// csharp
using Xunit;
using Moq;
using System.Threading.Tasks;
using KinoCentre.Controllers;
using KinoCentre.DB.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace KinoCentre.Tests
{
    public class TicketsControllerTests
    {
        [Fact]
        public async Task GetAllTicketsBySession_ReturnsOk_WithTicketList()
        {
            // Arrange
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.TicketsRepository.GetFilteredAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Ticket, bool>>>()))
                .ReturnsAsync(new List<Ticket> { new Ticket { Id = "123", SessionId = "S1" }});
            
            var controller = new TicketsController(mockUow.Object);

            // Act
            var result = await controller.GetAllTicketsBySession("S1") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var tickets = result.Value as List<Ticket>;
            Assert.Single(tickets);
            Assert.Equal("123", tickets.First().Id);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOk_WithTicket()
        {
            // Arrange
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.TicketsRepository.GetByIdAsync("123"))
                .ReturnsAsync(new Ticket { Id = "123" });
            var controller = new TicketsController(mockUow.Object);

            // Act
            var result = await controller.GetTicketById("123") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var ticket = result.Value as Ticket;
            Assert.Equal("123", ticket.Id);
        }

        [Fact]
        public async Task ValidateTicketById_ReturnsNotFound_WhenNoTicketMatches()
        {
            // Arrange
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.TicketsRepository.GetAllAsync())
                .ReturnsAsync(new List<Ticket>()); 
            var controller = new TicketsController(mockUow.Object);

            // Act
            var result = await controller.ValidateTicketById("999");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}