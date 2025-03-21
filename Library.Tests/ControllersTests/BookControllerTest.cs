using AutoFixture;
using FluentAssertions;
using Library.Controllers;
using Library.Models.DTOs;
using Library.Models.Responses;
using Library.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Library.Tests.ControllersTests
{
    public class BookControllerTest
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBookServices> _bookServicesMock;
        private readonly BookController _bookController; 

        public BookControllerTest()
        {
            _fixture = new Fixture();
            _bookServicesMock = new Mock<IBookServices>();
            _bookController = new BookController(_bookServicesMock.Object);
        }

        // GetAll()

        [Fact]
        public void GetAll_WhenServiceReturnsSuccess_ReturnsOkResult()
        {
            // Arrange
            var books = _fixture.CreateMany<BookDTO>(3).ToList();
            var serviceResponse = _fixture.Build<ServiceResponse<IEnumerable<BookDTO>>>()
                .With(x => x.Success, true)
                .With(x => x.Data, books)
                .With(x => x.Message, "Books retrieved successfully")
                .Create();

            _bookServicesMock.Setup(service => service.GetAll())
                .Returns(serviceResponse);

            // Act
            var result = _bookController.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(serviceResponse);

            var returnValue = okResult.Value as ServiceResponse<IEnumerable<BookDTO>>;
            returnValue.Success.Should().BeTrue();
            returnValue.Data.Should().HaveCount(3);
            returnValue.Message.Should().Be("Books retrieved successfully");
        }

        [Fact]
        public void GetAll_WhenServiceReturnsFail_ReturnsStatusCode500()
        {
            // Arrange
            var serviceResponse = _fixture.Build<ServiceResponse<IEnumerable<BookDTO>>>()
                .With(x => x.Success, false)
                .With(x => x.Data, (IEnumerable<BookDTO>)null)
                .With(x => x.Message, "Error retrieving books")
                .Create();

            _bookServicesMock.Setup(service => service.GetAll())
                .Returns(serviceResponse);

            // Act
            var result = _bookController.GetAll();

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var statusResult = result as ObjectResult;
            statusResult.StatusCode.Should().Be(500);

            var returnValue = statusResult.Value as ServiceResponse<IEnumerable<BookDTO>>;
            returnValue.Success.Should().BeFalse();
            returnValue.Data.Should().BeNull();
            returnValue.Message.Should().Be("Error retrieving books");
        }

        //GetById()
        [Fact]
        public void GetById_WhenBookExist_ShouldReturnOkResult()
        {
            //Arrange
            var book =  _fixture.Create<BookDTO>();
            int id = _fixture.Create<int>();
            var serviceResponse = _fixture.Build<ServiceResponse<BookDTO>>()
               .With(x => x.Success, true)
               .With(x => x.Data, book)
               .With(x => x.Message, "Book retrieved successfully")
               .Create();

            _bookServicesMock.Setup(s => s.GetById(id)).Returns(serviceResponse);

            //Act
            var result = _bookController.GetById(id);

            //Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnValue = okResult.Value as ServiceResponse<BookDTO>;
            returnValue.Success.Should().BeTrue();
            returnValue.Data.Should().BeEquivalentTo(book);
            returnValue.Message.Should().Be("Book retrieved successfully");

            _bookServicesMock.Verify(service => service.GetById(id), Times.Once);
        }
    }
}
