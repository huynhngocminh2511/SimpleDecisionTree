using Accord.Statistics.Analysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Controllers
{
    public class DataController
    {
        public static DataController Instance = new DataController();

        public static string pathFile = "Datas/a.arff.txt";

        public List<List<string>> Samples { private set; get; }

        public List<List<string>> TestSamples { private set; get; }

        public List<string> Labels { private set; get; }

        public List<string> Attributes { private set; get; }

        private DataController()
        {
            //Attribues = Enumerable.Range(0, 5).Select(x => x.ToString()).ToList();
        }

        public void LoadSamples(int percentDataTraining)
        {
            var samples = File.ReadAllLines(pathFile)
                .Select(x => x.Split(' ')
                    .ToList())
                .OrderBy(x => Guid.NewGuid())
                .ToList();

            var position = samples.Count() * percentDataTraining / 100;

            Console.WriteLine($"position: {position}");

            TestSamples = samples.Skip(position).ToList();

            File.WriteAllLines("Datas/test.txt", TestSamples
                .Select(x => x
                    .Aggregate((y, z) => y + z))
                .ToArray());
            

            Samples = samples.Take(position).ToList();

            File.WriteAllLines("Datas/train.txt", Samples
                .Select(x => x
                    .Aggregate((y, z) => y + z))
                .ToArray());

            Labels = samples.Select(x => x.Last()).ToList();

            Attributes = Enumerable.Range(0, samples.First().Count() - 1).Select(x => x.ToString()).ToList();

            var data = Samples.Select(x => x.Select(y => double.Parse(y)).ToArray()).ToArray();

            var principalComponentAnalysis = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center,
                Whiten = true
            };

            var transform = principalComponentAnalysis.Learn(data);


            var newdata = principalComponentAnalysis.Transform(data);

            newdata.ToList().ForEach(x =>
            {
                x.ToList().ForEach(y => Console.Write(y + " "));
                Console.WriteLine();
            });

            principalComponentAnalysis.NumberOfOutputs = 1;

            newdata = principalComponentAnalysis.Transform(data);

            newdata.ToList().ForEach(x =>
            {
                x.ToList().ForEach(y => Console.Write(y + " "));
                Console.WriteLine();
            });

            principalComponentAnalysis.ExplainedVariance = 0.8;

            newdata = principalComponentAnalysis.Transform(data);

            newdata.ToList().ForEach(x =>
            {
                x.ToList().ForEach(y => Console.Write(y + " "));
                Console.WriteLine();
            });

            var q = 1;
        }

        public List<List<string>> GetTestSamples()
        {
            TestSamples = File.ReadAllLines("Datas/test.arff.txt").Select(x =>
            {
                var sample = x.Split(' ').ToList();

                return sample;
            }).ToList();

            return TestSamples;
        }
    }
}
