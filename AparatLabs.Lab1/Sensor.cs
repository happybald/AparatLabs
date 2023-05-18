namespace AparatLabs.Lab1
{
    public class Sensor
    {
        public int Index { get; }
        public double Val { get; }
        public double High { get; }
        public double Low { get; }
        public double Delta { get; }
        public string DateTime { get; }

        public Sensor(int index, double val, double high, double low, double delta, string t)
        {
            Index = index;
            Val = val;
            High = high;
            Low = low;
            Delta = delta;
            DateTime = t;
        }

        public override string ToString() => $"Sensor{{Index={Index}, Value={Val}, High={High}, Low={Low}, Delta={Delta}, Time={DateTime}}}";
    }
}