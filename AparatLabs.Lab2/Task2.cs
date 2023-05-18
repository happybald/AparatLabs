using System.Diagnostics;
using SkiaSharp;
using static AparatLabs.Lab2.CalculationFunctions;
namespace AparatLabs.Lab2
{
    public static class Task2
    {
        public static async Task Run()
        {
            // Задаємо початкові значення констант
            const double qh = 1.0;
            const double qxmax = 4.0;
            // Довжина реалізації випадкового процесу, мм
            const double l = 100;
            // Швидкість руху діаграмного паперу самописця, мм/сек
            const double v = 10;
            // Кількість перетинів випадковим процесом лінії свого математичного очікування
            const double n = 100;
            const int size = 10;
            // Час, протягом якого відбулося N перетинів
            double tN = l / v;
            // Середнє число нулів за одиницю часу
            double nMiddle = n / tN;
            // Крок дискретизації випадкового процесу
            double deltaT = 0.15 / nMiddle;
            // Масив кроків дискретизації випадкового процесу
            double[] deltaTArr = GenerateDeltaTArr(deltaT, size);
            // Масив випадкових цілих чисел
            int[] arr = GenerateRandomIntArr(80, 140, size);
            // Математичне очікування
            double mx = (double)arr.Sum() / arr.Length;
            // Дисперсія
            double dx = CalcDx(arr, mx, n);
            // Масив значень потужності випадкового процесу
            double[] deltaQh = GenerateByValue(qh, size);
            // Масив максимальних значень потужності випадкового процесу
            double[] deltaQMax = GenerateByValue(qxmax, size);
            // Масив значень Kx
            double[] kx = CalcKx(deltaQh, deltaQMax, dx);
            // Значення J0
            double j0 = kx.Sum() / kx.Length;
            // Шуканий період опитування датчиків
            double t0 = j0 * deltaT;

            Console.WriteLine("2. Визначення періоду опитування датчиків за кривими реалізації випадкового процесу:");
            Console.WriteLine("а) Визначаємо крок дискретизації випадкового процесу:");
            Console.WriteLine($"Час, протягом якого відбулося {n} перетинів: {tN:F2} мм/сек");
            Console.WriteLine($"Середнє число нулів за одиницю часу: {nMiddle:F2}");
            Console.WriteLine($"Крок дискретизації Δτ: {deltaT:F2}");
            Console.WriteLine();
            Console.WriteLine("б) Визначаємо статистичні характеристики випадкового процесу:");
            Console.WriteLine($"Mx (Математичне очікування): {mx:F2}");
            Console.WriteLine($"Dx (Дисперсія): {dx:F2}");
            Console.WriteLine($"Kx (Кореляційна функція): [{string.Join(", ", kx.Select(x => x.ToString("F2")))}]");
            Console.WriteLine();
            Console.WriteLine("в) Визначаємо період опитування датчика:");
            Console.WriteLine($"t0 = {t0:F2}");
            Console.WriteLine($"(J0) = {j0:F2}");
            Console.WriteLine($"deltaTArr (Масив кроків дискретизації випадкового процесу): [{string.Join(", ", deltaTArr.Select(x => x.ToString("F2")))}]");

            Console.WriteLine("Отримати графік? (Масив значень Kx від кроків дискретизації випадкового процесу)");
            if (string.IsNullOrEmpty(Console.ReadLine()))
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var filePath = Path.Combine(documentsPath, "chart.png");
                DrawLineGraph(deltaTArr, kx, filePath);
                await OpenImageFile(filePath);
            }
        }

        // Метод для генерації масиву випадкових цілих чисел
        public static int[] GenerateRandomIntArr(int min, int max, int size = 13)
        {
            var random = new Random();
            return Enumerable.Range(0, size)
                .Select(_ => random.Next(min, max + 1))
                .ToArray();
        }

        // Метод для генерації масиву кроків дискретизації випадкового процесу
        public static double[] GenerateDeltaTArr(double deltaT, int size = 13)
        {
            double[] arr = new double[size];
            double value = deltaT;
            for (int i = 0; i < size; i++)
            {
                arr[i] = value;
                value += deltaT;
            }
            return arr;
        }

        // Метод для обчислення дисперсії
        public static double CalcDx(int[] arr, double mx, double n)
        {
            double sum = 0;
            foreach (int x in arr)
            {
                sum += Math.Pow(x - mx, 2);
            }
            return sum / (n - 1);
        }

        // Метод для обчислення масиву значень Kx
        public static double[] CalcKx(double[] deltaQh, double[] deltaQMax, double dx)
        {
            double[] kx = new double[deltaQh.Length];
            for (int i = 0; i < deltaQh.Length; i++)
            {
                kx[i] = CalcKxTa(dx, deltaQh[i], deltaQMax[i]);
            }
            return kx;
        }

        public static void DrawLineGraph(double[] xValues, double[] yValues, string filePath)
        {
            // Set up the graph size and padding
            int width = 900;// Width of the graph image
            int height = 900;// Height of the graph image
            // Create a new SKSurface and SKCanvas for drawing
            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            {
                var canvas = surface.Canvas;

                // Clear the canvas
                canvas.Clear(SKColors.White);// Define the size of the canvas
                SKRect canvasRect = new SKRect(0, 0, canvas.LocalClipBounds.Width, canvas.LocalClipBounds.Height);

                // Define the range of the x and y values
                double xMin = xValues.Min();
                double xMax = xValues.Max();
                double yMin = yValues.Min();
                double yMax = yValues.Max();

                // Define the size of the plot area
                SKRect plotRect = new SKRect(canvasRect.Left + 50, canvasRect.Top + 50, canvasRect.Right - 50, canvasRect.Bottom - 50);

                // Define the range of the plot area
                double plotWidth = plotRect.Width;
                double plotHeight = plotRect.Height;
                double xRange = xMax - xMin;
                double yRange = yMax - yMin;

                // Draw the coordinate axes
                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = SKColors.Black;
                    paint.StrokeWidth = 2;

                    // Draw the x-axis
                    canvas.DrawLine(plotRect.Left, plotRect.Bottom, plotRect.Right, plotRect.Bottom, paint);

                    // Draw the y-axis
                    canvas.DrawLine(plotRect.Left, plotRect.Top, plotRect.Left, plotRect.Bottom, paint);

                    // Draw the x-axis labels
                    for (double x = xMin; x <= xMax; x += xRange / 10)
                    {
                        float xCoord = (float)(plotRect.Left + (x - xMin) / xRange * plotWidth);
                        canvas.DrawLine(xCoord, plotRect.Bottom - 5, xCoord, plotRect.Bottom + 5, paint);
                        canvas.DrawText(x.ToString("F2"), xCoord, plotRect.Bottom + 20, paint);
                    }

                    // Draw the y-axis labels
                    for (double y = yMin; y <= yMax; y += yRange / 10)
                    {
                        float yCoord = (float)(plotRect.Bottom - (y - yMin) / yRange * plotHeight);
                        canvas.DrawLine(plotRect.Left - 5, yCoord, plotRect.Left + 5, yCoord, paint);
                        canvas.DrawText(y.ToString("F2"), plotRect.Left - 40, yCoord, paint);
                    }
                }

                // Draw the grid
                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = SKColors.LightGray;
                    paint.StrokeWidth = 1;

                    // Draw the vertical grid lines
                    for (double x = xMin; x <= xMax; x += xRange / 10)
                    {
                        float xCoord = (float)(plotRect.Left + (x - xMin) / xRange * plotWidth);
                        canvas.DrawLine(xCoord, plotRect.Top, xCoord, plotRect.Bottom, paint);
                    }

                    // Draw the horizontal grid lines
                    for (double y = yMin; y <= yMax; y += yRange / 10)
                    {
                        float yCoord = (float)(plotRect.Bottom - (y - yMin) / yRange * plotHeight);
                        canvas.DrawLine(plotRect.Left, yCoord, plotRect.Right, yCoord, paint);
                    }
                }

                // Draw the line graph
                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = SKColors.Blue;
                    paint.StrokeWidth = 1;

                    for (int i = 0; i < xValues.Length - 1; i++)
                    {
                        double x1 = xValues[i];
                        double y1 = yValues[i];
                        double x2 = xValues[i + 1];
                        double y2 = yValues[i + 1];
                        float x1Coord = (float)(plotRect.Left + (x1 - xMin) / xRange * plotWidth);
                        float y1Coord = (float)(plotRect.Bottom - (y1 - yMin) / yRange * plotHeight);
                        float x2Coord = (float)(plotRect.Left + (x2 - xMin) / xRange * plotWidth);
                        float y2Coord = (float)(plotRect.Bottom - (y2 - yMin) / yRange * plotHeight);
                        canvas.DrawLine(x1Coord, y1Coord, x2Coord, y2Coord, paint);
                    }
                }

                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = SKColors.Black;
                    paint.TextSize = 30;
                    paint.IsAntialias = true;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

                    // Define the watermark text
                    string watermarkText = "Danylo Serhieiev";

                    // Calculate the size of the watermark text
                    SKRect watermarkRect = new SKRect();
                    paint.MeasureText(watermarkText, ref watermarkRect);

                    // Calculate the position of the watermark text
                    float watermarkX = canvasRect.Right - watermarkRect.Width - 50;
                    float watermarkY = canvasRect.Top + watermarkRect.Height + 50;// adjust the Y position

                    // Draw the watermark text
                    canvas.DrawText(watermarkText, watermarkX, watermarkY, paint);
                }


                // Save the image to a file
                using (var image = surface.Snapshot())
                using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
                using (var stream = File.OpenWrite(filePath))
                {
                    data.SaveTo(stream);
                }
            }
        }

        public static async Task OpenImageFile(string filePath)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true
            };

            using var process = Process.Start(processStartInfo);
            if (process != null)
            {
                await Task.Run(() => process.WaitForExit());
            }
        }
    }
}
