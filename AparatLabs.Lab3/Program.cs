using System.Text;
namespace AparatLabs.Lab3;


public static class Program
{
    //У методі Main виконуються основні розрахунки. Спочатку визначається час T за допомогою методу GetT.
    //Потім розраховуються значення кореляційних функцій Kx за допомогою методів.
    //Нарешті, розраховуються значення технологічних параметрів для екстраполяції і інтерполяції.
    //Результати виводяться на консоль.
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        //Математичне очікування
        double mx = 205;
        //Значення параметру на і-му циклі вимірювання
        double xti = 220;
        //Дисперсія
        double dx = 30;
        //Тривалість циклу tц=τ0 (пері од опитування датчика)
        double t0 = 10;
        //Час затухання авто- кореляційної функції
        double t = 20;
        //Параметр K
        double k = 1.5;
        //Період прогнозу, t (момент часу, в котрий необхідно визначити величину)
        double multiply = 0.5;

        //повертає значення часу T, яке використовується в інших розрахунках
        double T = multiply * t0;

        //розраховує значення кореляційної функції Kx за формулою (3.9) з вхідних даних
        double kx = dx * (1 - Math.Pow(T / t0, k));
        //розраховує значення кореляційної функції Kx за формулою (3.11) з вхідних даних.
        double kxInterpolation = dx * Math.Exp(-t0 / T);

        //розраховує значення технологічного параметру за формулою з вхідних даних.
        double extrapolation = kx / dx * (xti - mx) + mx;
        double interpolation = kxInterpolation / dx * (xti - mx) + mx;

        Console.WriteLine($"T = {T:F4}");
        Console.WriteLine($"Kx екстраполяції = {kx:F4}");
        Console.WriteLine($"Kx інтерполяції = {kxInterpolation:F4}");
        Console.WriteLine($"X екстраполяції: {extrapolation:F4}");
        Console.WriteLine($"X інтерполяції: {interpolation:F4}");
    }
}
