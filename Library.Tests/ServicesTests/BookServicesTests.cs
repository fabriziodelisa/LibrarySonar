using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Library.Models.DTOs;
using Library.Models.Entities;
using Library.Repositories;
using Library.Services;
using Moq;

namespace Library.Tests.ServicesTests
{
    public class BookServicesTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly BookServices _bookServices;
        private readonly Mock<IMapper> _mapperMock;
        public BookServicesTests()
        {
            _fixture = new Fixture();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _mapperMock = new Mock<IMapper>();
            _bookServices = new BookServices(_bookRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetAll_WhenBooksExists_ShouldReturnBooks()
        {
            //Arrange
            var books = _fixture.CreateMany<Book>(3).ToList();
            var bookDTOs = _fixture.CreateMany<BookDTO>(3).ToList();

            _bookRepositoryMock.Setup(repo => repo.GetAll()).Returns(books);
            _mapperMock.Setup(m => m.Map<IEnumerable<BookDTO>>(books)).Returns(bookDTOs);

            //Act
            var result = _bookServices.GetAll();

            //Assert
            result.Success.Should().BeTrue();
            result.Data.Should().HaveCount(3);  
            result.Message.Should().Be("Books retrieved successfully");1
            _bookRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenRepositoryThrowsException_ShouldReturnErrorResponse()
        {
            // Arrange
            var exceptionMessage = "Database connection failed";
            _bookRepositoryMock.Setup(repo => repo.GetAll()).Throws(new Exception(exceptionMessage));

            // Act
            var result = _bookServices.GetAll();

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Error retrieving books");
            result.Errors.Should().Contain(exceptionMessage);
        }

        [Fact]
        public void GetById_WhenBookExists_ShouldReturnBook()
        {
            //Arrange
            var book = _fixture.Create<Book>();
            var bookDTO = _fixture.Create<BookDTO>();
            var id = It.IsAny<int>();

            _bookRepositoryMock.Setup(repo => repo.GetById(id)).Returns(book);
            _mapperMock.Setup(m => m.Map<BookDTO>(book)).Returns(bookDTO);

            //Act
            var result = _bookServices.GetById(id);

            //Assert
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Book retrieved successfully");
            result.Data.Should().BeEquivalentTo(bookDTO);
            _bookRepositoryMock.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Fact]
        public void GetById_WhenBookDoesntExists_ShouldReturnNotFoundMessage()
        {
            //Arrange
            var book = (Book)null;
            var bookDTO = (BookDTO)null;
            var id = It.IsAny<int>();

            _bookRepositoryMock.Setup(repo => repo.GetById(id)).Returns(book);
            _mapperMock.Setup(m => m.Map<BookDTO>(book)).Returns(bookDTO);

            //Act
            var result = _bookServices.GetById(id);

            //Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be($"Book with ID {id} was not found");
            result.Data.Should().BeNull();
        }

        [Fact]
        public void GetById_WhenRepositoryThrowsException_ShouldReturnErrorResponse()
        {
            // Arrange
            var id = It.IsAny<int>();
            var exceptionMessage = "Database connection failed";
            _bookRepositoryMock.Setup(repo => repo.GetById(id))
                .Throws(new Exception(exceptionMessage));

            // Act
            var result = _bookServices.GetById(id);

            // Assert
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Error retrieving book");
            result.Errors.Should().Contain(exceptionMessage);
        }
    }
}
