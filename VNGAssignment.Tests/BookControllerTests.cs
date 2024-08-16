using FluentAssertions;
using VNGAssignment;
using System.Net;
using Moq;
using VNGAssignment.Services;
using VNGAssignment.Controllers;
using VNGAssignment.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using VNGAssignment.Models;

namespace VNGAssignment.Tests
{
    public class BookControllerTests
    {
        private Mock<IBookService> bookServiceMock;

        public BookControllerTests()
        {
            bookServiceMock = new Mock<IBookService>();
        }

        [Fact]
        public async Task GetById_WhenBookExists()
        {
            int bookId = 1;
            var book = new Book { Id = bookId, Title = "How to Win Friends and Influence People", Author = "Dale Carnegie", PublishedYear = "1936" };

            bookServiceMock.Setup(exp => exp.GetById(bookId)).ReturnsAsync(book);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.GetById(1);

            response.Should().BeOfType<OkObjectResult>();
            
            var result = (OkObjectResult)response;
            result.Value.Should().BeOfType<Book>().Which.Should().BeEquivalentTo(book);
        }

        [Fact]
        public async Task GetById_WhenBookDoesNotExist()
        {
            int bookId = 1;

            bookServiceMock.Setup(exp => exp.GetById(bookId)).ReturnsAsync((Book)null);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.GetById(1);

            response.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAll_WithListOfBooks()
        {
            var books = new List<Book>
            {
                new() { Id = 1, Title = "How to Win Friends and Influence People", Author = "Dale Carnegie", PublishedYear = "1936" },
                new() { Id = 2, Title = "Think and Grow Rich", Author = "Napoleon Hill", PublishedYear = "1937" },
                new() { Id = 3, Title = "The 7 Habits of Highly Effective People", Author = "Stephen R. Covey", PublishedYear = "1989" }
            };

            bookServiceMock.Setup(exp => exp.GetAll()).ReturnsAsync(books);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.GetAll();

            response.Should().BeOfType<OkObjectResult>();

            var result = (OkObjectResult)response;
            result.Value.Should().BeOfType<List<Book>>().Which.Should().BeEquivalentTo(books);
        }

        [Fact]
        public async Task GetAll_WhenNoBooksExist()
        {
            bookServiceMock.Setup(service => service.GetAll()).ReturnsAsync(new List<Book>());
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.GetAll();

            response.Should().BeOfType<OkObjectResult>();

            var result = (OkObjectResult)response;
            result.Value.Should().BeOfType<List<Book>>().Which.Should().BeEmpty();
        }

        [Fact]
        public async Task Create_WithNewBook()
        {
            var newBook = new AddBookRequest { Title = "New Book", Author = "New Author", PublishedYear = "2023" };
            var createdBook = new Book { Id = 1, Title = "New Book", Author = "New Author", PublishedYear = "2023" };

            bookServiceMock.Setup(service => service.Create(newBook)).ReturnsAsync(createdBook);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.Create(newBook);

            response.Should().BeOfType<OkObjectResult>();

            var result = (OkObjectResult)response;
            result.Value.Should().BeOfType<Book>().Which.Should().BeEquivalentTo(createdBook);
        }

        [Fact]
        public async Task Update_WhenUpdateIsSuccessful()
        {
            var updateBook = new UpdateBookRequest { Id = 1, Title = "Updated Title", Author = "Updated Author", PublishedYear = "2021" };

            bookServiceMock.Setup(service => service.Update(updateBook)).ReturnsAsync(true);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.Update(updateBook);

            response.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Update_WhenBookDoesNotExist()
        {
            var updateBook = new UpdateBookRequest { Id = 1, Title = "Updated Title", Author = "Updated Author", PublishedYear = "2021" };

            bookServiceMock.Setup(service => service.Update(updateBook)).ReturnsAsync(false);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.Update(updateBook);

            response.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_WhenDeleteIsSuccessful()
        {
            int bookId = 1;

            bookServiceMock.Setup(service => service.Delete(bookId)).ReturnsAsync(true);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.Delete(bookId);

            response.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_WhenBookDoesNotExist()
        {
            int bookId = 1;

            bookServiceMock.Setup(service => service.Delete(bookId)).ReturnsAsync(false);
            var bookController = new BookController(bookServiceMock.Object);

            var response = await bookController.Delete(bookId);

            response.Should().BeOfType<NotFoundResult>();
        }
    }
}