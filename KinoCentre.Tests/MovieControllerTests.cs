// csharp
using Xunit;
using Moq;
using System.Threading.Tasks;
using KinoCentre.Controllers;
using KinoCentre.DB.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KinoCentre.Tests
{
    public class MoviesControllerTests
    {
        [Fact]
        public async Task GetAllPaginatedMovies_ReturnsOk_WithMovieList()
        {
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.MoviesRepository.GetAllAsync())
                .ReturnsAsync(new List<Movie> { new Movie { Id = "1", Title = "Test" } });

            var controller = new MoviesController(mockUow.Object);

            var result = await controller.GetAllPaginatedMovies() as OkObjectResult;

            Assert.NotNull(result);
            var movies = result.Value as List<Movie>;
            Assert.Single(movies);
            Assert.Equal("Test", movies[0].Title);
        }

        [Fact]
        public async Task GetMovieById_ReturnsOk_WithCorrectMovie()
        {
            var mockUow = new Mock<UnitOfWork>(null);
            mockUow.Setup(u => u.MoviesRepository.GetByIdAsync("1"))
                .ReturnsAsync(new Movie { Id = "1", Title = "Some Movie" });

            var controller = new MoviesController(mockUow.Object);

            var result = await controller.GetMovieById("1") as OkObjectResult;

            Assert.NotNull(result);
            var movie = result.Value as Movie;
            Assert.Equal("1", movie.Id);
        }
    }
}