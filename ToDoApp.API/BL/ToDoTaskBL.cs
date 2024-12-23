using Microsoft.EntityFrameworkCore;
using Serilog;
using ToDoApp.Common.Models;
using ToDoApp.DB;

namespace ToDoApp.API.BL;

public class ToDoTaskBL : IToDoTaskBL
{
    private readonly ApplicationDbContext _dbContext;
    public ToDoTaskBL(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;   
    }
    public async Task<ToDoTask> CreateTaskAsync(ToDoTask task)
    {
        ToDoTask result = new();
        ValidateTask(task);

        try
        {
            var model = new ToDoTask()
            {
                Title = task.Title,
                Description = task.Description,
                ExpiryAt = task.ExpiryAt,
                PercentCompleted = 0,
                IsCompleted = false
            };
            await _dbContext.TblTask.AddAsync(model);
            await _dbContext.SaveChangesAsync();

            result = model;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<int> DeleteTaskAsync(int id)
    {
        var result = 0;
        try
        {
            var taskToDelete = await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == id);

            if (taskToDelete != null)
            {
                _dbContext.TblTask.Remove(taskToDelete);
                await _dbContext.SaveChangesAsync();
                result = id;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<List<ToDoTask>?> GetAllTasksAsync()
    {
        List<ToDoTask>? result = null;

        try
        {
            result = await _dbContext.TblTask.ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<ToDoTask?> GetTaskByIdAsync(int id)
    {
        ToDoTask? result = null;

        try
        {
            result = await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == id);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<List<ToDoTask>?> GetTasksByDateAsync(DateTimeOffset? from, DateTimeOffset to)
    {
        List<ToDoTask>? result = null;

        try
        {
            result = await _dbContext.TblTask
                .Where(x => (!from.HasValue || x.CreatedAt >= from.Value) && x.CreatedAt <= to)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<int> SetTaskAsCompletedAsync(int id)
    {
        var result = 0;

        try
        {
            var task = await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == id);

            if (task != null)
            {
                task.IsCompleted = true;
                task.PercentCompleted = 100;
                await _dbContext.SaveChangesAsync();
                result = id;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<int> UpdatePercentCompletedAsync(ToDoTask task)
    {
        var result = 0;

        try
        {
            var existingTask = await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == task.Id);
            ValidateTask(task);

            if (existingTask != null)
            {
                existingTask.PercentCompleted = task.PercentCompleted;
                existingTask.IsCompleted = task.PercentCompleted == 100;
                await _dbContext.SaveChangesAsync();
                result = task.Id;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    public async Task<ToDoTask> UpdateTaskAsync(ToDoTask task)
    {
        ToDoTask result = new();

        try
        {
            var existingTask = await _dbContext.TblTask.FirstOrDefaultAsync(x => x.Id == task.Id);

            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.ExpiryAt = task.ExpiryAt;
                existingTask.PercentCompleted = task.PercentCompleted;
                existingTask.IsCompleted = task.IsCompleted;
                await _dbContext.SaveChangesAsync();
                result = task;
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }

        return result;
    }

    private static void ValidateTask(ToDoTask task)
    {
        if (task.ExpiryAt <= DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("Expiry date must be in the future.", nameof(task.ExpiryAt));
        }

        if (task.PercentCompleted < 0 || task.PercentCompleted > 100)
        {
            throw new ArgumentException("Percent completed must be between 0 and 100.", nameof(task.PercentCompleted));
        }
    }
}
