namespace AparatLabs.Lab1
{
    public class Sensor
    {
        public int Index { get; }
        public double Value { get; }
        public double High { get; }
        public double Low { get; }
        public double Delta { get; }
        public string DateTime { get; }

        public Sensor(int index, double value, double high, double low, double delta, string t)
        {
            Index = index;
            Value = value;
            High = high;
            Low = low;
            Delta = delta;
            DateTime = t;
        }

        public override string ToString() => $"Sensor{{Index={Index}, Value={Value}, High={High}, Low={Low}, Delta={Delta}, Time={DateTime}}}";
    }
}