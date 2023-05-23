using System.Net.NetworkInformation;
using static AparatLabs.Lab2.CalculationFunctions;
namespace AparatLabs.Lab2
{
    public static class Task1
    {
        public static void Run()
        {
            // Задаємо початкові значення для розрахунків.
            const double qh = 1.0;
            const double qxmax = 4.0;
            const double t0Temp = 2;
            const double dtemp = 44;
            const double t0Press = 0.1;
            const double dpress = 24;

            double[] qhValues = GenerateByValue(qh, 5);
            double[] qxmaxValues = GenerateByValue(qxmax, 5);

            // Розраховуємо величину Кх(То) за формулою (2.9).
            double kxTemp = CalcKxTa(dtemp, qh, qxmax);

            // Генеруємо масив дельта Т
            double[] deltaT = GenerateByValue(t0Temp, 5);
            // Розраховуємо масив значень Ктх(Т, ΔQх)
            double[] ktxTempTDeltaQh = qhValues.Select(x => CalcKxTa(dtemp, x, qxmax)).ToArray();
            // Розраховуємо масив значень Ктх(Т, ΔQmax)
            double[] ktxTempTDeltaQMax = qxmaxValues.Select(x => CalcKxTa(dtemp, qh, x)).ToArray();

            // Розраховуємо величину Кх(То) за формулою (2.9) для тиску.
            double kxPress = CalcKxTa(dpress, qh, qxmax);

            // Генеруємо масив дельта Р
            double[] pDeltaP = GenerateByValue(t0Press, 5, 50);
            // Розраховуємо масив значень Ктх(Р, ΔQх)
            double[] ktxPressTDeltaQh = qhValues.Select(x => CalcKxTa(dpress, x, qxmax)).ToArray();
            // Розраховуємо масив значень Ктх(Р, ΔQmax)
            double[] ktxPressTDeltaQMax = qxmaxValues.Select(x => CalcKxTa(dpress, qh, x)).ToArray();

            Console.WriteLine("1. Визначення періоду опитування датчиків з реалізації випадкових процесів за температурою і тиском");
            Console.WriteLine("a) Величина Kx(Ta) при заданих QxMax та Qh:");
            Console.WriteLine($"Kx(Ta) = {kxTemp:F2}");
            Console.WriteLine();
            Console.WriteLine("б) За графіком кореляційних функцій, визначаємо значення періодів опитування T0 датчиків температури і тиску:");
            Console.WriteLine($"T0 для датчика температури: {t0Temp:F2}");
            Console.WriteLine($"T0 для датчика тиску: {t0Press:F2}");
            Console.WriteLine();
            Console.WriteLine("в) Для постійної величини QxMax визначаємо декілька значень T0 при різних значеннях Qh для датчика температури:");
            Console.WriteLine($"Kx(T0) = {kxTemp:F2}");
            Console.WriteLine("QxMax = const");
            for (int i = 0; i < deltaT.Length; i++)
            {
                Console.WriteLine($"K(T - {i}) = {ktxTempTDeltaQh[i]:F4}, T = {deltaT[i]:F2}");
            }
            Console.WriteLine();
            Console.WriteLine("г) Для постійної величини Qh визначаємо декілька значень T0 при різних значеннях QxMax для датчика температури:");
            Console.WriteLine("Qh = const");
            for (int i = 0; i < deltaT.Length; i++)
            {
                Console.WriteLine($"K(T - {i}) = {ktxTempTDeltaQMax[i]:F4}, T = {deltaT[i]:F2}");
            }
            Console.WriteLine();
            Console.WriteLine("Датчик тиску:");
            Console.WriteLine("в) Для постійної величини QxMax визначаємо декілька значень T0 при різних значеннях Qh:");
            Console.WriteLine($"Kx(T0) = {kxPress:F2}");
            Console.WriteLine("QxMax = const");
            for (int i = 0; i < pDeltaP.Length; i++)
            {
                Console.WriteLine($"K(T - {i}) = {ktxPressTDeltaQh[i]:F4}, T = {pDeltaP[i]:F2}");
            }
            Console.WriteLine();
            Console.WriteLine("г) Для постійної величини Qh визначаємо декілька значень T0 при різних значеннях QxMax:");
            Console.WriteLine("Qh = const");
            for (int i = 0; i < pDeltaP.Length; i++)
            {
                Console.WriteLine($"K(T - {i}) = {ktxPressTDeltaQMax[i]:F4}, T = {pDeltaP[i]:F2}");
            }
            Console.WriteLine("д) Можна зробити висновок, що статичні властивості вимірюваної величини (похибки її визначення і похибки вимірювального тракту) впливають на величину Т0.");
            Console.WriteLine("Зокрема, при збільшенні похибок вимірювання і похибок вимірювального тракту, значення Т0 збільшується.");
            Console.WriteLine("Також можна зробити висновок, що збільшення значення Qh при постійному значенні QxMax призводить до зменшення значення Т0, а збільшення значення QxMax при постійному значенні Qh призводить до збільшення значення Т0.");
            Console.WriteLine();
        }

       
    }
}
