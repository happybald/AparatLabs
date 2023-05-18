using System.Text;
namespace AparatLabs.Lab2;


public class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Task1.Run();
        await Task2.Run();


//Chart
        /*string[] propertyValues = Array.ConvertAll(deltaTArr, el => el.ToString("0.00"));
        Console.WriteLine(propertyValues.GetType());
        var chart = new Chart();
        chart.Setting.Add(new ChartSetting("type", "line"));
        chart.Data.Labels.AddRange(propertyValues);
        var dataset = new ChartDataset();
        dataset.Label = "Chart";
        dataset.Fill = false;
        dataset.BorderColor = "rgb(75, 192, 192)";
        dataset.Tension = 0.1;
        dataset.Data.AddRange(Kx);
        chart.Data.Datasets.Add(dataset);
        chart.Options.Scales.Y.BeginAtZero = true;
        Console.WriteLine(chart.ToJson());
//Render chart in HTML page
        var html = "<html><head><script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script></head><body><canvas id=\"myChart\"></canvas><script>var ctx = document.getElementById(\"myChart\").getContext(\"2d\");var myChart = new Chart(ctx, " + chart.ToJson() + ");</script></body></html>";
        Console.WriteLine(html);*/
    }
}
