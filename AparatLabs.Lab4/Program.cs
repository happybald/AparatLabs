using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Spectre.Console;
using System.Text;
namespace AparatLabs.Lab4;

public static class CalculationUtils
{
    public static double CalcCoefficientL(double q1, double q2, double q3, double q4)
    {
        return q1 + q2 + q3 - q4;
    }

    public static (double a1, double a2, double a3, double a4) CalcACoefficients(double l, double q1, double q2, double q3, double q4)
    {
        return (
            a1: l / q1 > 0 ? 1 : -1,
            a2: l / q2 > 0 ? 1 : -1,
            a3: l / q3 > 0 ? 1 : -1,
            a4: l / q4 > 0 ? 1 : -1
            );
    }

    public static double[] CalcDeltaQ((double a1, double a2, double a3, double a4) aCoefficients, double q1, double q2, double q3, double q4)
    {
        return new double[] { q1 * aCoefficients.a1, q2 * aCoefficients.a2, q3 * aCoefficients.a3, q4 * aCoefficients.a4 };
    }

    public static (double k, double p1, double p2, double p3, double p4) CalcPCoefficients(double sigma1, double sigma2, double sigma3, double sigma4)
    {
        double sigma1Squared = Math.Pow(sigma1, 2);
        double sigma2Squared = Math.Pow(sigma2, 2);
        double sigma3Squared = Math.Pow(sigma3, 2);
        double sigma4Squared = Math.Pow(sigma4, 2);
        double k = 1 / ((1 / sigma1Squared) + (1 / sigma2Squared) + (1 / sigma3Squared) + (1 / sigma4Squared));
        double p1 = k / sigma1Squared;
        double p2 = k / sigma2Squared;
        double p3 = k / sigma3Squared;
        double p4 = k / sigma4Squared;
        return (k, p1, p2, p3, p4);
    }

    public static double[] SolveSystem((double k, double p1, double p2, double p3, double p4) pCoefficients, double[] deltaQ, double coefficientL)
    {
        var p1 = pCoefficients.p1;
        var p2 = pCoefficients.p2;
        var p3 = pCoefficients.p3;
        var p4 = pCoefficients.p4;

        var a = DenseMatrix.OfArray(new double[,]
        {
            { 2 * p1 * deltaQ[0], 0, 0, 0, 1 },
            { 0, 2 * p2 * deltaQ[1], 0, 0, 1 },
            { 0, 0, 2 * p3 * deltaQ[2], 0, 1 },
            { 0, 0, 0, 2 * p4 * deltaQ[3], 1 },
            { deltaQ[0], deltaQ[1], deltaQ[2], -deltaQ[3], 0 },
        });

        var b = DenseVector.OfArray(new double[] { 0, 0, 0, 0, coefficientL });

        var resultVector = a.Solve(b);
        return resultVector.ToArray()[1..];
    }

    public static double[] CalcRefactoredValues(double q1, double q2, double q3, double q4, double[] xs)
    {
        return new double[] { q1 - xs[0], q2 - xs[1], q3 - xs[2], q4 + xs[3] };
    }
}
public static class Program
{
    public static void Main()
    {

        Console.OutputEncoding = Encoding.UTF8;

        double x1 = 12.1;
        double x2 = 11.6;
        double x3 = 12.4;
        double x4 = 34.5;
        double l = 1.5;
        double sigma1 = 0.32;
        double sigma2 = 0.20;
        double sigma3 = 0.36;
        double sigma4 = 0.33;
        double[] deltaX = new double[] { 0.45, 0.45, 0.45, 0.65 };
        double coefficientL = CalculationUtils.CalcCoefficientL(x1, x2, x3, x4);

        bool isCorrect = Math.Abs(coefficientL) < l;

        var aCoefficients = CalculationUtils.CalcACoefficients(l, x1, x2, x3, x4);
        var deltaQ = CalculationUtils.CalcDeltaQ(aCoefficients, x1, x2, x3, x4);
        var pCoefficients = CalculationUtils.CalcPCoefficients(sigma1, sigma2, sigma3, sigma4);
        var solvedSystem = CalculationUtils.SolveSystem(pCoefficients, deltaQ, coefficientL);

        var refactoredValues = CalculationUtils.CalcRefactoredValues(x1, x2, x3, x4, solvedSystem);
        double sumRefactored = refactoredValues.Sum();

        Console.WriteLine("1. Визначаємо похибку l виконання рівняння зв'язку між виміряними параметрами:");
        Console.WriteLine($"l: {coefficientL:0.0000}");
        Console.WriteLine();
        Console.WriteLine("2. Перевіряємо виконання умови:");
        Console.WriteLine("l > l*");
        Console.WriteLine($"{l:0.0000} > {coefficientL:0.0000}");
        if (isCorrect)
        {
            Console.WriteLine("Все добре");
        }
        else
        {
            Console.WriteLine("Отже, серед результатів вимірювання Xi є не достовірні.");
            Console.WriteLine("3. Запишемо лінеаризовану математичну модель процесу, для якої знайдемо числові значення коефіцієнтів:");
            Console.WriteLine($"A1 = {aCoefficients.a1:0.0000}");
            Console.WriteLine($"A2 = {aCoefficients.a2:0.0000}");
            Console.WriteLine($"A3 = {aCoefficients.a3:0.0000}");
            Console.WriteLine($"A4 = {aCoefficients.a4:0.0000}");
            Console.WriteLine("4. Запишемо систему рівнянь для чого розрахуємо спочатку вагові коефіцієнти р:");
            Console.WriteLine($"K = {pCoefficients.k:0.0000}");
            Console.WriteLine($"P1 = {pCoefficients.p1:0.0000}");
            Console.WriteLine($"P2 = {pCoefficients.p2:0.0000}");
            Console.WriteLine($"P3 = {pCoefficients.p3:0.0000}");
            Console.WriteLine($"P4 = {pCoefficients.p4:0.0000}");
            Console.WriteLine("5. Результат рішення системи рівнянь:");
            Console.WriteLine($"deltaQ1 = {solvedSystem[0]:0.0000}");
            Console.WriteLine($"deltaQ2 = {solvedSystem[1]:0.0000}");
            Console.WriteLine($"deltaQ3 = {solvedSystem[2]:0.0000}");
            Console.WriteLine($"deltaQ4 = {solvedSystem[3]:0.0000}");
            Console.WriteLine("6. Перевіряємо виконання умови |ΔXi| ≤ xi*.");
            for (var i = 0; i < solvedSystem.Length; i++)
            {
                var value = solvedSystem[i];
                if (Math.Abs(value) > deltaX[i])
                {
                    Console.WriteLine($"x{i + 1}: Умова не виконується для значення {value:0.0000}");
                }
                else
                {
                    Console.WriteLine($"x{i + 1}: Умова виконується для значення {value:0.0000}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("7. За формулою (4.17) розраховуємо скориговані оцінки значень вимірюваних величин:");
            Console.WriteLine($"Q1 = {refactoredValues[0]:0.0000}");
            Console.WriteLine($"Q2 = {refactoredValues[1]:0.0000}");
            Console.WriteLine($"Q3 = {refactoredValues[2]:0.0000}");
            Console.WriteLine($"Q4 = {refactoredValues[3]:0.0000}");
            Console.WriteLine($"8. З урахуванням скоригованих значень перевіримо знову виконання умови:");
            Console.WriteLine(sumRefactored <= l ? "Умова не виконується" : "Умова виконується");


            var table = new Table();
            table.AddColumn("Step");
            table.AddColumn("Result");

            table.AddRow("1", $"l: {coefficientL:0.0000}");
            table.AddEmptyRow();
            table.AddRow("2", "Check condition:");
            table.AddRow("", $"l > l*");
            table.AddRow("", $"{l:0.0000} > {coefficientL:0.0000}");
            if (isCorrect)
            {
                table.AddRow("", "Everything is fine");
            }
            else
            {
                table.AddRow("", "Отже, серед результатів вимірювання x̃i є не достовірні.");
                table.AddEmptyRow();
                table.AddRow("3", "Запишемо лінеаризовану математичну модель процесу, для якої знайдемо числові значення коефіцієнтів");
                table.AddRow("", $"A1 = {aCoefficients.a1:0.0000}");
                table.AddRow("", $"A2 = {aCoefficients.a2:0.0000}");
                table.AddRow("", $"A3 = {aCoefficients.a3:0.0000}");
                table.AddRow("", $"A4 = {aCoefficients.a4:0.0000}");
                table.AddEmptyRow();
                table.AddRow("4", "Запишемо систему рівнянь для чого розрахуємо спочатку вагові коефіцієнти р");
                table.AddRow("", $"K = {pCoefficients.k:0.0000}");
                table.AddRow("", $"P1 = {pCoefficients.p1:0.0000}");
                table.AddRow("", $"P2 = {pCoefficients.p2:0.0000}");
                table.AddRow("", $"P3 = {pCoefficients.p3:0.0000}");
                table.AddRow("", $"P4 = {pCoefficients.p4:0.0000}");
                table.AddEmptyRow();
                table.AddRow("5", "Результат рішення системи рівнянь");
                table.AddRow("", $"deltaQ1 = {solvedSystem[0]:0.0000}");
                table.AddRow("", $"deltaQ2 = {solvedSystem[1]:0.0000}");
                table.AddRow("", $"deltaQ3 = {solvedSystem[2]:0.0000}");
                table.AddRow("", $"deltaQ4 = {solvedSystem[3]:0.0000}");
                table.AddEmptyRow();
                table.AddRow("6", "Перевіряємо виконання умови |Δxi| ≤ xi*");
                for (var i = 0; i < solvedSystem.Length; i++)
                {
                    var value = solvedSystem[i];
                    if (Math.Abs(value) > deltaX[i])
                    {
                        table.AddRow("", $"x{i + 1}: Умова не виконується для значення {value:0.0000}");
                    }
                    else
                    {
                        table.AddRow("", $"x{i + 1}: Умова виконується для значення {value:0.0000}");
                    }
                }
                table.AddEmptyRow();
                table.AddRow("7", " За формулою (4.17) розраховуємо скориговані оцінки значень вимірюваних величин");
                table.AddRow("", $"Q1 = {refactoredValues[0]:0.0000}");
                table.AddRow("", $"Q2 = {refactoredValues[1]:0.0000}");
                table.AddRow("", $"Q3 = {refactoredValues[2]:0.0000}");
                table.AddRow("", $"Q4 = {refactoredValues[3]:0.0000}");
                table.AddEmptyRow();
                table.AddRow("8", "З урахуванням скоригованих значень перевіримо знову виконання умови");
                table.AddRow("", sumRefactored <= l ? "Умова не виконується" : "Умова виконується");
            }

            AnsiConsole.Write(table);
        }
    }
}
