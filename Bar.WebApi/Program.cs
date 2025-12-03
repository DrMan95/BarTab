using BarBillHolderLibrary.Models;
using BarBillHolderLibrary.Database;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// === Dynamic base path (no hard-coded C:\ paths) ===
// This is the root folder of your Bar.WebApi project at runtime.
string basePath = builder.Environment.ContentRootPath;
// FileProcessor will look in basePath\Data and basePath\History
FileProcessor.InitializeFilePath(basePath);

// Load menu + bar from files under basePath\Data
FileProcessor.ReadMenuFromCSV();

Debug.Print(basePath);
if (FileProcessor.FileBarIsEmpty())
{
    BarBillHolderLibrary.Models.Bar.InitializeBar("BarakiBar");
    Debug.Print("INITIALIZED BAR (no saved data)");
}
else
{
    FileProcessor.ParseFileBar();
    Debug.Print("LOADED BAR FROM FILE");
}

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
