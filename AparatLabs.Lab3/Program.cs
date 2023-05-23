using System.Text;
namespace AparatLabs.Lab3;

public static class Program
{
    //Метод GetT приймає два параметри - початковий час t0 і множник multiply, і повертає значення часу T, яке використовується в інших розрахунках.
    private static double GetT(double t0, double multiply) => multiply * t0;

    //Метод CalcKx приймає параметри dx, t, t0 і k, і розраховує значення кореляційної функції Kx за формулою (3.9) з вхідних даних.
    private static double CalcKx(double dx, double t, double t0, double k) => dx * (1 - Math.Pow(t / t0, k));

    //Метод CalcKxInterpolation приймає параметри dx, t0 і T, і розраховує значення кореляційної функції Kx за формулою (3.11) з вхідних даних.
    private static double CalcKxInterpolation(double dx, double t0, double T) => dx * Math.Exp(-t0 / T);

    //Метод CalcResult приймає параметри kx, dx, xti і mx, і розраховує значення технологічного параметру за формулою з вхідних даних.
    private static double CalcResult(double kx, double dx, double xti, double mx) => kx / dx * (xti - mx) + mx;

    //У методі Main виконуються основні розрахунки. Спочатку визначається час T за допомогою методу GetT.
    //Потім розраховуються значення кореляційних функцій Kx за допомогою методів CalcKx і CalcKxInterpolation.
    //Нарешті, розраховуються значення технологічних параметрів за допомогою методу CalcResult для екстраполяції і інтерполяції.
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
        
        double T = GetT(t0, multiply);

        double kx = CalcKx(dx, T, t0, k);
        double kxInterpolation = CalcKxInterpolation(dx, t0, T);

        double extrapolation = CalcResult(kx, dx, xti, mx);
        double interpolation = CalcResult(kxInterpolation, dx, xti, mx);

        Console.WriteLine($"T = {T:F4}");
        Console.WriteLine($"Kx екстраполяції = {kx:F4}");
        Console.WriteLine($"Kx інтерполяції = {kxInterpolation:F4}");
        Console.WriteLine($"X екстраполяції: {extrapolation:F4}");
        Console.WriteLine($"X інтерполяції: {interpolation:F4}");
    }
}
