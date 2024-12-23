using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Common.Models;

namespace ToDoApp.Tests.Integration;

// to fix System.InvalidOperationException : Can't find 'ToDoApp.Tests\bin\Debug\net8.0\testhost.deps.json'. This file is required for functional tests to run properly. There should be a copy of the file on your source project bin folder. If that is not the case, make sure that the property PreserveCompilationContext is set to true on your project file. E.g '<PreserveCompilationContext>true</PreserveCompilationContext>'. For functional tests to work they need to either run from the build output folder or the testhost.deps.json file from your application's output directory must be copied to the folder where the tests are running on. A common cause for this error is having shadow copying enabled when the tests run.

public class ToDoControllerIntegrationTests : ApiIntegrationTestBase
{
    public ToDoControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    //[Fact]
    public async Task GetAllTasks_ShouldReturnTasks()
    {
        // act
        var response = await Client.GetAsync("/ToDoController/get-all");

        // assert
        response.EnsureSuccessStatusCode();
        var tasks = await response.Content.ReadFromJsonAsync<List<ToDoTask>>();
        Assert.NotNull(tasks);
        Assert.True(tasks.Count >= 2);
    }

    //[Fact]
    public async Task CreateTask_ShouldAddTask()
    {
        // arrange
        var newTask = new ToDoTask
        {
            Title = "New Task",
            Description = "New Description",
            ExpiryAt = DateTimeOffset.UtcNow.AddDays(3)
        };

        // act
        var response = await Client.PostAsJsonAsync("/ToDoController/create", newTask);

        // assert
        response.EnsureSuccessStatusCode();
        var createdTask = await response.Content.ReadFromJsonAsync<ToDoTask>();
        Assert.NotNull(createdTask);
        Assert.Equal(newTask.Title, createdTask.Title);
    }

    //[Fact]
    public async Task SetTaskAsCompleted_ShouldUpdateTask()
    {
        // arrange
        var taskId = 1;

        // act
        var response = await Client.PostAsync($"/ToDoController/set-as-completed?id={taskId}", null);

        // assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>();
        Assert.Equal(taskId, result);
    }

    //[Fact]
    public async Task DeleteTask_ShouldRemoveTask()
    {
        // arrange
        var taskId = 1;

        // act
        var response = await Client.DeleteAsync($"/ToDoController/delete?id={taskId}");

        // assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<int>();
        Assert.Equal(taskId, result);
    }
}
