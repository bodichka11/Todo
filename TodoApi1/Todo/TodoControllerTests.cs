using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TodoApi1.Controllers;
using TodoApi1;

namespace Todo
{
    public class TodoControllerTests
    {
        private readonly TodoController _controller;
        private readonly TodoContext _context;

        public TodoControllerTests()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                    .UseInMemoryDatabase(databaseName: "TodoListDb")
                    .Options;
            _context = new TodoContext(options);
            _context.Database.EnsureDeleted(); // Clears the database
            _context.Database.EnsureCreated(); // Recreates the database
            _controller = new TodoController(_context);
        }

        #region Test Create

        [Fact]
        public async Task PostTodoItem_CreatesTodoItem()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Test Todo", Description = "Test Description" };

            // Act
            var result = await _controller.PostTodoItem(todoItem);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result); // Access the specific result type
            Assert.NotNull(createdResult);
            Assert.Equal(201, createdResult.StatusCode); // 201 Created

            var returnValue = Assert.IsType<TodoItem>(createdResult.Value);
            Assert.NotNull(returnValue);
            Assert.Equal("Test Todo", returnValue.Title);
            Assert.Equal("Test Description", returnValue.Description);
        }

        #endregion

        #region Test Read (Get All Items)

        [Fact]
        public async Task GetTodoItems_ReturnsAllTodoItems()
        {
            // Arrange
            _context.TodoItems.Add(new TodoItem { Title = "Todo 1", Description = "Description 1" });
            _context.TodoItems.Add(new TodoItem { Title = "Todo 2", Description = "Description 2" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var todoItems = result.Value as List<TodoItem>;
            todoItems.Should().HaveCount(2);
        }

        #endregion

        #region Test Read (Get Single Item)

        [Fact]
        public async Task GetTodoItem_ReturnsTodoItem_WhenItemExists()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Todo 1", Description = "Description 1" };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTodoItem(todoItem.Id);

            // Assert
            var item = result.Value;
            item.Should().NotBeNull();
            item.Title.Should().Be(todoItem.Title);
            item.Description.Should().Be(todoItem.Description);
        }

        [Fact]
        public async Task GetTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Act
            var result = await _controller.GetTodoItem(999);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Test Update

        [Fact]
        public async Task PutTodoItem_UpdatesTodoItem_WhenItemExists()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Todo 1", Description = "Description 1" };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            todoItem.Description = "Updated Description";

            // Act
            var result = await _controller.PutTodoItem(todoItem.Id, todoItem);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var updatedItem = await _context.TodoItems.FindAsync(todoItem.Id);
            updatedItem.Should().NotBeNull();
            updatedItem.Description.Should().Be("Updated Description");
        }

        [Fact]
        public async Task PutTodoItem_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Todo 1", Description = "Description 1" };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            var updatedItem = new TodoItem { Id = 999, Title = "Todo 999", Description = "Description 999" };

            // Act
            var result = await _controller.PutTodoItem(todoItem.Id, updatedItem);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task PutTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Act
            var result = await _controller.PutTodoItem(999, new TodoItem { Id = 999, Title = "Not Found", Description = "Description" });

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion

        #region Test Delete

        [Fact]
        public async Task DeleteTodoItem_DeletesTodoItem_WhenItemExists()
        {
            // Arrange
            var todoItem = new TodoItem { Title = "Todo 1", Description = "Description 1" };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteTodoItem(todoItem.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            var deletedItem = await _context.TodoItems.FindAsync(todoItem.Id);
            deletedItem.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTodoItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteTodoItem(999);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        #endregion
    }
}