using Bar.WebApi.Data;
using BarBillHolderLibrary;
using BarBillHolderLibrary.Database;
using BarBillHolderLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using BarState = BarBillHolderLibrary.Models.Bar;

var builder = WebApplication.CreateBuilder(args);

// Controllers + camelCase JSON (so JS sees id/name/open/total)
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQLite EF Core
var connectionString = $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "bar.db")}";
builder.Services.AddDbContext<BarDbContext>(options => options.UseSqlite(connectionString));

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Serve index.html from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.MapGet("/debug/state", async (BarDbContext db) =>
{
    var menuCount = await db.MenuItems.CountAsync();
    var activeMenuCount = await db.MenuItems.CountAsync(m => m.Active);
    var tablesCount = BarBillHolderLibrary.Models.Bar.tables?.Count ?? 0;

    return Results.Ok(new { menuCount, activeMenuCount, tablesCount });
});

// IMPORTANT: initialize/load Bar state from file before requests come in
InitializeBarState(app.Environment.ContentRootPath);

// Seed DB menu
await MenuSeeder.SeedMenuAsync(app.Services);

app.Run();

static void InitializeBarState(string contentRootPath)
{
    FileProcessor.InitializeFilePath(contentRootPath);

    if (FileProcessor.FileBarIsEmpty())
    {
        BarState.name = "BarakiBar";
        BarState.register = new Register(0m, 0m, 0m);
        BarState.customers = new List<Customer>();
        BarState.tables = new List<Table>();

        for (int i = 1; i <= 14; i++)
            BarState.tables.Add(new Table(i));

        FileProcessor.SaveBarInstanceAsync().GetAwaiter().GetResult();
    }
    else
    {
        FileProcessor.ParseFileBar();
    }

    BarState.tables ??= new List<Table>();

    for (int i = 1; i <= 14; i++)
        if (!BarState.tables.Any(t => t.ID == i))
            BarState.tables.Add(new Table(i));

    BarState.tables = BarState.tables.OrderBy(t => t.ID).ToList();
    BarState.register ??= new Register(0m, 0m, 0m);
}
