using System.Text;
namespace AparatLabs.Lab5;


public static class Program
{
    //Термопара хромель-копель
    public static double CalculateTemperature(double outputSignal, double yMax, double resolution, double T)
    {
        double acpMax = Math.Pow(2, resolution);
        double knp = outputSignal / yMax;
        double y = (outputSignal / (acpMax * knp)) * T;
        return 3.01 + 13.75 * y - 0.03 * Math.Pow(y, 2);
    }

    public static double CalculatePressure(double P, double outputSignal, double resolution)
    {
        double acpMax = Math.Pow(2, resolution);
        return (P / acpMax) * outputSignal;
    }

    public static double CalculateSpending(double t, double p, double p0, double F, double fMax, double resolution)
    {
        double acpMax = Math.Pow(2, resolution);
        double pG = 1.2 - 0.013 * t + 0.72 * p + 0.000036 * Math.Pow(t, 2) + 0.0024 * Math.Pow(p, 2) - 0.0014 * t * p;
        double kp = Math.Sqrt(pG / p0);
        return Math.Sqrt(F / acpMax) * fMax * kp;
    }
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        double outputSignal = 10;
        //За градуювальними таблицями маємо ymax = 16.39
        double yMax = 16.39;
        double resolution = 10;
        double T = 512;
        double p0 = 2.88;
        double F = 256;
        double fMax = 500;
        double P = 768;

        double temperature = CalculateTemperature(outputSignal, yMax, resolution, T);
        double pressure = CalculatePressure(P, outputSignal, resolution);

        double spending = CalculateSpending(temperature, pressure, p0, F, fMax, resolution);

        Console.WriteLine("За вказаними в таблицях 5.2-5.11 даними, проведемо обчислення фактичних значень вимірюваних величин, таких як тиск, температура і витрата");
        Console.WriteLine($"Температура: {temperature:F4}");
        Console.WriteLine($"Тиск: {pressure:F4}");
        Console.WriteLine($"Витрата: {spending:F4}");
    }
}
