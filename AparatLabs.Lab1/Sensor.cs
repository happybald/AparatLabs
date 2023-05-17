namespace AparatLabs.Lab1
{
    public class Sensor
    {
        public int Index { get; }
        public double Val { get; }
        public double High { get; }
        public double Low { get; }
        public double A { get; }
        public string T { get; }

        public Sensor()
        {
        }

        public Sensor(int index, double val, double high, double low, double a, string t)
        {
            Index = index;
            Val = val;
            High = high;
            Low = low;
            A = a;
            T = t;
        }

        public override string ToString()
        {
            return $"Sensor{{index={Index}, val={Val}, high={High}, low={Low}, a={A}, t={T}}}";
        }
    }
}
