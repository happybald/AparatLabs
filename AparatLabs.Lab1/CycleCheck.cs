namespace AparatLabs.Lab1;

public class CycleCheck
{
    public static void Main1(string[] args)
    {
        var highLimit = Enumerable.Range(0, 10)
            .Select(_ => new Random().NextDouble() * (100 - 60) + 60)//from 60 to 100
            .ToList();
        var lowLimit = Enumerable.Range(0, 10)
            .Select(_ => new Random().NextDouble() * 40)//from 0 to 40
            .ToList();
        var delta = Enumerable.Range(0, 10)
            .Select(_ => new Random().NextDouble() * 5)//from 0 to 40
            .ToList();
        var sensors = new List<Sensor>();
        var sensorsInfo = new List<Sensor>();
        var j = 0;

        for (var i = 1; i < 11; i++)
        {
            var rand = new Random().NextDouble() * (highLimit[i - 1] + delta[i - 1] - (lowLimit[i - 1] - delta[i - 1])) + (lowLimit[i - 1] - new Random().NextDouble() * (5 + -5) + -5);
            var sensor = new Sensor(i, Math.Round(rand), highLimit[i - 1], lowLimit[i - 1], delta[i - 1], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sensors.Add(sensor);

            var deltaXh = sensor.High - sensor.Val;
            var deltaXl = sensor.Low - sensor.Val;

            //в рамках дельти
            if (deltaXh < 0 || deltaXl > 0)
            {
                sensorsInfo.Insert(j, sensor);
                j++;
                //За рамки дельти
                if (sensor.Val > sensor.High + sensor.Delta || sensor.Val < sensor.Low - sensor.Delta)
                {
                    var defaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"WARN! {sensor}");
                    Console.ForegroundColor = defaultColor;
                }
            }
        }

        Console.WriteLine("Nicht gut sensors");
        while (j > 0)
        {
            Console.WriteLine(sensorsInfo[j - 1]);
            j--;
        }
        Console.WriteLine("All sensors");
        sensors.ForEach(Console.WriteLine);
    }
}
