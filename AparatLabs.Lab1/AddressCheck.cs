namespace AparatLabs.Lab1;
public static class AddressCheck
{
    public static void Main(string[] args)
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
        var p = new List<int> { 5, 3, 4, 2, 7 };
        var priority = new List<int> { 0, 5, 1, 2, 3 };

        var sensorsByPriority = InsertSensorsData();
        var sensorsInfo = new Dictionary<int, Sensor>();
        var j = 0;
        var k = 0;

        Console.WriteLine("1: Poll the detector by number");
        Console.WriteLine("2: Targeted polling");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.Write("Enter scanner index: ");
                var index = int.Parse(Console.ReadLine());
                Console.WriteLine("Checking " + index + " sensor!");
                var sensor = sensorsByPriority.Keys.First(el => el.Index == index);
                k++;
                CheckSensor(sensor);
                break;
            case "2":
                foreach (var entry in sensorsByPriority)
                {
                    Console.WriteLine("Checking " + entry.Key.Index + " sensor!");
                    k++;
                    CheckSensor(entry.Key);
                }
                break;
            default: return;
        }

        Console.WriteLine("Nicht gut sensors");
        while (j > 0)
        {
            Console.WriteLine(sensorsInfo[j - 1]);
            j--;
        }

        Console.WriteLine("All sensors");

        foreach ((var sensor, _) in sensorsByPriority)
            Console.WriteLine(sensor);

        void CheckSensor(Sensor sensor)
        {
            var deltaXh = sensor.High - sensor.Value;
            var deltaXl = sensor.Low - sensor.Value;
            Thread.Sleep(p[k - 1] * 1000);
            //в рамках дельти
            if (deltaXh < 0 || deltaXl > 0)
            {
                sensorsInfo.Add(j, sensor);
                j++;
                //За рамки дельти
                if (sensor.Value > sensor.High + sensor.Delta || sensor.Value < sensor.Low - sensor.Delta)
                {
                    var defaultColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"WARN! {sensor}");
                    Console.ForegroundColor = defaultColor;

                }
            }
        }

        Dictionary<Sensor, int> InsertSensorsData()
        {
            var sensorByPriority = new Dictionary<Sensor, int>();
            for (var i = 1; i < 6; i++)
            {
                var rand = new Random().NextDouble() * (highLimit[i - 1] + delta[i - 1] - (lowLimit[i - 1] - delta[i - 1])) + (lowLimit[i - 1] - new Random().NextDouble() * (5 + -5) + -5);
                var sensor = new Sensor(i, Math.Round(rand), highLimit[i - 1], lowLimit[i - 1], delta[i - 1], DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sensorByPriority.Add(sensor, priority[i - 1]);
            }
            return sensorByPriority.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
