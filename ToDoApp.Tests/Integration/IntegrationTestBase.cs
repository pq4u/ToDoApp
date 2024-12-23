using Microsoft.EntityFrameworkCore;
using ToDoApp.Common.Models;
using ToDoApp.DB;

namespace ToDoApp.Tests.Integration;

public abstract class IntegrationTestBase : IDisposable
{
    protected readonly ApplicationDbContext DbContext;

    protected IntegrationTestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique db for each test
            .Options;

        DbContext = new ApplicationDbContext(options);
        SeedData();
    }

    private void SeedData()
    {
        DbContext.TblTask.AddRange(
            new ToDoTask { Title = "Task 1", Description = "Description 1", ExpiryAt = DateTimeOffset.UtcNow.AddDays(1) },
            new ToDoTask { Title = "Task 2", Description = "Description 2", ExpiryAt = DateTimeOffset.UtcNow.AddDays(2) }
        );
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}