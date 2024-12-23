using ToDoApp.Common.Models;

namespace ToDoApp.BL
{
    public interface IToDoTaskBL
    {
        Task<ToDoTask> CreateTaskAsync(ToDoTask task);
        Task<ToDoTask> UpdateTaskAsync(ToDoTask task);
        Task<List<ToDoTask>?> GetAllTasksAsync();
        Task<ToDoTask?> GetTaskByIdAsync(int id);
        Task<List<ToDoTask>?> GetTasksByDateAsync(DateTimeOffset? from, DateTimeOffset to);
        Task<int> UpdatePercentCompletedAsync(ToDoTask task);
        Task<int> SetTaskAsCompletedAsync(int id);
        Task<int> DeleteTaskAsync(int id);

    }
}
