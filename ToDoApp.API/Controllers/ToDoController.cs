using Microsoft.AspNetCore.Mvc;
using ToDoApp.BL;
using ToDoApp.Common.Models;

namespace ToDoApp.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ToDoController : ControllerBase
{
    private readonly IToDoTaskBL _toDoTaskBL;

    public ToDoController(IToDoTaskBL toDoTaskBL)
    {
        _toDoTaskBL = toDoTaskBL;
    }

    [HttpGet]
    [Route("get-all")]
    public async Task<IActionResult> GetAllTasksAsync()
    {
        var result = await _toDoTaskBL.GetAllTasksAsync();
        return Ok(result);
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetTaskByIdAsync(int id)
    {
        var result = await _toDoTaskBL.GetTaskByIdAsync(id);
        return result != null ? Ok(result) : NotFound(id);
    }

    [HttpGet]
    [Route("get-by-date")]
    public async Task<IActionResult> GetTasksByDateAsync(DateTimeOffset? from, DateTimeOffset to)
    {
        var result = await _toDoTaskBL.GetTasksByDateAsync(from, to);
        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateTaskAsync(ToDoTask task)
    {
        var result = await _toDoTaskBL.CreateTaskAsync(task);
        return Ok(result);
    }

    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> UpdateTaskAsync(ToDoTask task)
    {
        var result = await _toDoTaskBL.UpdateTaskAsync(task);
        return result != null ? Ok(result) : NotFound(task);
    }

    [HttpGet]
    [Route("update-percent-completed")]
    public async Task<IActionResult> UpdatePercentCompletedAsync(ToDoTask task)
    {
        var result = await _toDoTaskBL.UpdatePercentCompletedAsync(task);
        return result == 0 ? Ok(result) : NotFound(task);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> DeleteTaskAsync(int id)
    {
        var result = await _toDoTaskBL.DeleteTaskAsync(id);
        return result == 0 ? Ok(result) : NotFound(id);
    }

    [HttpPost]
    [Route("set-as-completed")]
    public async Task<IActionResult> SetTaskAsCompletedAsync(int id)
    {
        var result = await _toDoTaskBL.SetTaskAsCompletedAsync(id);
        return result == 0 ? Ok(result) : NotFound(id);
    }
}
