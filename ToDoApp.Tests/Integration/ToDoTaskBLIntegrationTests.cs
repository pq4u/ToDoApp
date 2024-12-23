using ToDoApp.BL;
using ToDoApp.Common.Models;

namespace ToDoApp.Tests.Integration;

public class ToDoTaskBLIntegrationTests : IntegrationTestBase
{
    private readonly ToDoTaskBL _toDoTaskBL;

    public ToDoTaskBLIntegrationTests()
    {
        _toDoTaskBL = new ToDoTaskBL(DbContext);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldAddTask()
    {
        // arrange
        var task = new ToDoTask
        {
            Title = "New Task",
            Description = "New Description",
            ExpiryAt = DateTimeOffset.UtcNow.AddDays(5)
        };

        // act
        var result = await _toDoTaskBL.CreateTaskAsync(task);

        // assert
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(task.Title, result.Title);
        Assert.Equal(3, DbContext.TblTask.Count());
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // act
        var tasks = await _toDoTaskBL.GetAllTasksAsync();

        // assert
        Assert.NotNull(tasks);
        Assert.Equal(2, tasks.Count);
    }

    [Fact]
    public async Task SetTaskAsCompletedAsync_ShouldMarkTaskAsCompleted()
    {
        // arrange
        var task = DbContext.TblTask.First();
        Assert.False(task.IsCompleted);

        // act
        var result = await _toDoTaskBL.SetTaskAsCompletedAsync(task.Id);

        // assert
        Assert.Equal(task.Id, result);
        var updatedTask = await DbContext.TblTask.FindAsync(task.Id);
        Assert.True(updatedTask!.IsCompleted);
        Assert.Equal(100, updatedTask.PercentCompleted);
    }
}