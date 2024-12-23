using Serilog;
using ToDoApp.Common;
using ToDoApp.DB;
using Microsoft.EntityFrameworkCore;
using ToDoApp.API.BL;

Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    var configuration = builder.Configuration;

    string connectionString = configuration.GetConnectionString(CommonConstants.ToDoDbConnection)!;

    builder.Host.UseSerilog((ctx, lc) => lc
        .WriteTo.Console()
        .ReadFrom.Configuration(ctx.Configuration));

    builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseNpgsql(connectionString));

    builder.Services.AddScoped<IToDoTaskBL, ToDoTaskBL>();

    //builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddControllers();

    builder.Services.AddSwaggerGen(options =>
    {
        options.EnableAnnotations();
    });

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}