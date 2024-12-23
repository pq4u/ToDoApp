using Microsoft.EntityFrameworkCore;
using ToDoApp.Common.Models;

namespace ToDoApp.DB;

public class ApplicationDbContext : DbContext
{
    public DbSet<ToDoTask> TblTask { get; set; }


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

}