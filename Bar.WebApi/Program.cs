using BarBillHolderLibrary.Models;
using BarBillHolderLibrary.Database;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

string filePath = @"C:\Users\nmarm\source\repos";
FileProcessor.InitializeFilePath(filePath);
FileProcessor.ReadMenuFromCSV();

Debug.Print(filePath);
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

if (BarBillHolderLibrary.Models.Bar.menu == null || BarBillHolderLibrary.Models.Bar.menu.Count == 0)
{
    Debug.Print("MENU WAS EMPTY - INITIALIZING TEST MENU");
    BarBillHolderLibrary.Models.Bar.menu = new List<Tuple<string, List<Tuple<string, decimal>>>>()
    {
        Tuple.Create("Coffee", new List<Tuple<string, decimal>>
        {
            Tuple.Create("Espresso", 2.50m),
            Tuple.Create("Cappuccino", 3.00m),
            Tuple.Create("Freddo Espresso", 3.50m),
        }),
        Tuple.Create("Beer", new List<Tuple<string, decimal>>
        {
            Tuple.Create("Draft Beer", 4.00m),
            Tuple.Create("Bottled Beer", 4.50m),
        }),
        Tuple.Create("Snacks", new List<Tuple<string, decimal>>
        {
            Tuple.Create("Chips", 2.00m),
            Tuple.Create("Nuts", 2.50m),
        }),
    };
}
// ============================================================

// Serve wwwroot/index.html at "/"
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
