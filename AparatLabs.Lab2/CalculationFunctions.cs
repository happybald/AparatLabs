namespace AparatLabs.Lab2
{
    public static class CalculationFunctions
    {
        // Метод CalcKxTa розраховує величину Кх(Та) за формулою (2.9).
        public static double CalcKxTa(double dx, double qh, double qx)
        {
            return (2 * dx + Math.Pow(qh, 2) - Math.Pow(qx, 2)) / 2;
        }

        // Метод GenerateByValue генерує масив значень за заданою формулою.
        // Параметр size вказує розмір масиву, delimiter - крок між значеннями.
        public static double[] GenerateByValue(double value, int size = 5, double delimiter = 10)
        {
            double[] arr = new double[size];
            for (int i = 0; i < arr.Length; i++)
            {
                if (i % 2 == 0)
                {
                    arr[i] = value + i / delimiter;
                }
                else
                {
                    arr[i] = value - i / delimiter;
                }
            }
            Array.Sort(arr);
            return arr;
        }
    }
}
