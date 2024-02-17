using BarBillHolderLibrary.Models;
using BarBillHolderLibrary.Database;

namespace BarBillHolderUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        //////////////////[STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            string filePath = @"C:\Users\Mattheos\source\repos";
            FileProcessor.InitializeFilePath(filePath);
            FileProcessor.ReadMenuFromCSV();
            if (FileProcessor.FileBarIsEmpty())
            {
                Bar.InitializeBar("BarakiBar");
            }
            else
            {
                FileProcessor.ParseFileBar();
            }
            Application.Run(new StartingPage());
            _ = FileProcessor.SaveBarInstanceAsync();
            FileProcessor.SaveMenuToCSV();
        }
    }
}