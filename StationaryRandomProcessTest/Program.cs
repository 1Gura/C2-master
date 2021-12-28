using System;
using System.Collections.Generic;
using System.Linq;
using WindowsFormsApp1;
namespace StationaryRandomProcessTest
{
    public class Program
    {
        static void FisherCriterion(List<double> a, List<double> b, int N)
        {
            var m1 = a.Sum() / N;
            var m2 = b.Sum() / N;
            var d1 = 0d;
            var d2 = 0d;
            for (var i = 0; i < N; i++)
            {
                d1 += Math.Pow(a[i] - m1, 2) / (N - 1);
                d2 += Math.Pow(b[i] - m2, 2) / (N - 1);
            }
            double fisherCriterion = 0;
            if (d2 < d1)
            {
                fisherCriterion = d1 / d2;
            }
            else
            {
                fisherCriterion = d2 / d1;
            }
            var beta = N - 1;
            var alpha = 0.05;
            OutputInfoFisher(m1, m2, d1, d2, fisherCriterion, alpha, beta);
        }
        static void OutputInfoFisher(double m1, double m2, double d1, double d2, double fisherCriterion, double alpha, double beta)
        {
            Console.WriteLine($"Параметры для расчета Критерия согласия Фишера");
            Console.WriteLine($"m'x - {m1}");
            Console.WriteLine($"m''y - {m2}");
            Console.WriteLine($"Dx - {d1}");
            Console.WriteLine($"Dy - {d2}");
            Console.WriteLine($"Критерий значимости - {alpha}");
            Console.WriteLine($"Степеней свободы - {beta}");
            Console.WriteLine($"Критерий Фишера - {fisherCriterion}");
        }

        static void StudentCriterion(List<double> a, List<double> b, int N)
        {
            var m1 = a.Sum() / N;
            var m2 = b.Sum() / N;
            var d1 = 0d;
            var d2 = 0d;
            for (var i = 0; i < N; i++)
            {
                d1 += Math.Pow(a[i] - m1, 2) / (N - 1);
                d2 += Math.Pow(b[i] - m2, 2) / (N - 1);
            }

            var D = ((N - 1) * d1 + (N - 1) * d2) / (N - 2);
            double studentCriterion = Math.Sqrt((Math.Pow(m1 - m2, 2) * N * N) / (D * N * 2));
            var beta = N + N - 2;
            OutputInfoStudent(a, b, m1, m2, d1, d2, D, studentCriterion, beta);
        }

        static void OutputInfoStudent(List<double> a, List<double> b, double m1, double m2, double d1, double d2, double D, double studentCriterion, double beta)
        {
            Console.WriteLine($"Фрагмент чётных сгенерированных значений - [{string.Join(", ", a.Take(50))}]");
            Console.WriteLine($"Фрагмент нечётных сгенерированных значений - [{string.Join(", ", b.Take(50))}]");
            Console.WriteLine($"Параметры для расчета Критерия согласия Стьюдента");
            Console.WriteLine($"m'x - {m1}");
            Console.WriteLine($"m''y - {m2}");
            Console.WriteLine($"Dx - {d1}");
            Console.WriteLine($"Dy - {d2}");
            Console.WriteLine($"D - {D}");
            Console.WriteLine($"Степеней свободы - {beta}");
            Console.WriteLine($"Критерий Стьюдента - {studentCriterion}");
        }


        static void Main(string[] args)
        {
            var generator = new StationaryRandomProcess(
                new double[] { 0.05355541672, 0.1258679265, 0.3421478267, 0.9296364597 },
                1, 10);
            var N = 1000;
            var a = new List<double>();
            var b = new List<double>();
            for (var i = 0; i < N * 2; ++i)
            {
                if (i % 2 == 0)
                {
                    a.Add(generator.GetNextValue());
                }
                else
                {
                    b.Add(generator.GetNextValue());
                }
            }

            StudentCriterion(a, b, N);
            FisherCriterion(a, b, N);
            _ = Console.ReadKey();
        }
    }
}
