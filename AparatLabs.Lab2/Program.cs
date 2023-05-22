using System.Text;
namespace AparatLabs.Lab2;


public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Task1.Run();
        Console.ReadLine();
        await Task2.Run();
    }
}
