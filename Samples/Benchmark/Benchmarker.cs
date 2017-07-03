using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    public static class Benchmarker
    {
        public static void Run(Action action, int iterations)
        {
            Run(action.Method.Name, action, iterations);
        }

        public static void Run(string name, Action action, int iterations)
        {
            var sampleCount = 10;

            var sw = new System.Diagnostics.Stopwatch();

            for (var i = iterations / 5 * 4; i < iterations; i++)
            {
                action();
            }

            var samples = new long[sampleCount];

            for (var s = 0; s < sampleCount; s++)
            {
                sw.Restart();
                for (var i = 0; i < iterations; i++)
                {
                    action();
                }
                sw.Stop();

                samples[s] = sw.ElapsedMilliseconds;
            }
            
            Console.WriteLine("{0}: {1:0.00} ms", name, (double)samples.Min() / iterations);
        }
    }
}
