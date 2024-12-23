using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.API.BL;
using ToDoApp.Common.Models;
using ToDoApp.DB;

namespace ToDoApp.Tests.Unit;

public class ToDoTaskBLUnitTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ToDoTaskBL _toDoTaskBL;

    public ToDoTaskBLUnitTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique db for each test
                .Options;

        _dbContext = new ApplicationDbContext(options);
        _toDoTaskBL = new ToDoTaskBL(_dbContext);
    }
    [Fact]
    public async Task CreateTaskAsync_ShouldAddTaskToDatabase()
    {
        // arrange
        var task = new ToDoTask
        {
            Title = "Test Task",
            Description = "Description",
            ExpiryAt = DateTimeOffset.UtcNow.AddDays(1)
        };

        // act
        var result = await _toDoTaskBL.CreateTaskAsync(task);

        // assert
        var savedTask = await _dbContext.TblTask.FirstOrDefaultAsync();
        Assert.NotNull(savedTask);
        Assert.Equal(task.Title, savedTask.Title);
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        // arrange
        _dbContext.TblTask.AddRange(new ToDoTask { Title = "Task 1" }, new ToDoTask { Title = "Task 2" });
        await _dbContext.SaveChangesAsync();

        // act
        var result = await _toDoTaskBL.GetAllTasksAsync();

        // assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // arrange
        var task = new ToDoTask { Title = "Task 1" };
        _dbContext.TblTask.Add(task);
        await _dbContext.SaveChangesAsync();

        // act
        var result = await _toDoTaskBL.GetTaskByIdAsync(task.Id);

        // assert
        Assert.NotNull(result);
        Assert.Equal(task.Title, result.Title);
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // act
        var result = await _toDoTaskBL.GetTaskByIdAsync(2025);

        // assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldRemoveTask_WhenTaskExists()
    {
        // arrange
        var task = new ToDoTask { Title = "Task to delete" };
        _dbContext.TblTask.Add(task);
        await _dbContext.SaveChangesAsync();

        // act
        var result = await _toDoTaskBL.DeleteTaskAsync(task.Id);

        // assert
        Assert.Equal(task.Id, result);
        Assert.Null(await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == task.Id));
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnZero_WhenTaskDoesNotExist()
    {
        // act
        var result = await _toDoTaskBL.DeleteTaskAsync(999);

        // assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task SetTaskAsCompletedAsync_ShouldUpdateTaskStatus()
    {
        // arrange
        var task = new ToDoTask { Title = "Incomplete Task", PercentCompleted = 50 };
        _dbContext.TblTask.Add(task);
        await _dbContext.SaveChangesAsync();

        // act
        var result = await _toDoTaskBL.SetTaskAsCompletedAsync(task.Id);

        // assert
        Assert.Equal(task.Id, result);
        var updatedTask = await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == task.Id);
        Assert.NotNull(updatedTask);
        Assert.True(updatedTask.IsCompleted);
        Assert.Equal(100, updatedTask.PercentCompleted);
    }
}
