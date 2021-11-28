using System;

namespace WindowsFormsApp1
{
    public class Generator
    {
        private static readonly Random ExponentialRandom = new Random();

        private static readonly Random NormalRandom = new Random();

        public static double ExponentialDistributionFunction(double lambda)
            => -(1 / lambda) * Math.Log(ExponentialRandom.NextDouble());

        public static double NormalDistributionFunction(double sigma, double m)
            => (sigma * Math.Cos(2 * Math.PI * NormalRandom.NextDouble())
                * Math.Sqrt(-2 * Math.Log(NormalRandom.NextDouble()))) + m;
    }
}
