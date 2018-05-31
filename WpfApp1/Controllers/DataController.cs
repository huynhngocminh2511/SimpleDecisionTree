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
                .Select(x => x.Split(new char[] { ' ', ',' })
                    .ToList())
                .OrderBy(x => Guid.NewGuid())//merge
                .ToList();

            var position = samples.Count() * percentDataTraining / 100;//split train(position)/test(remain)

            //Console.WriteLine($"position: {position}");

            TestSamples = samples.Skip(position).ToList();//dataset train

            //File.WriteAllLines("Datas/test.txt", TestSamples
            //    .Select(x => x
            //        .Aggregate((y, z) => y + z))
            //    .ToArray());


            Samples = samples.Take(position).ToList();//dataset test

            //File.WriteAllLines("Datas/train.txt", Samples
            //    .Select(x => x
            //        .Aggregate((y, z) => y + z))
            //    .ToArray());

            Labels = samples.Select(x => x.Last()).ToList();

            Attributes = Enumerable.Range(0, samples.First().Count() - 1).Select(x => x.ToString()).ToList();

            //var q = 1;
            //ReduceAttribute(percentDataTraining);
        }

        public void SelectAttribute()
        {

        }

        public void ReduceAttribute(int percentDataTraining)
        {
            //Samples.tak

            var labels = Samples.Select(x => x.Last()).ToList();

            var data = Samples.Select(x => 
            {
                x.RemoveAt(x.Count() - 1);
                return x.Select(y => double.Parse(y)).ToArray();
            }).ToArray();

            var principalComponentAnalysis = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center,
                //Means = mean,
                Whiten = true
            };

            principalComponentAnalysis.Learn(data);

            //var transform = 

            //Console.WriteLine();
            //principalComponentAnalysis.Eigenvalues.ToList().ForEach(x => Console.Write(x + " "));
            //Console.WriteLine();

            //Console.WriteLine();
            //principalComponentAnalysis.ComponentProportions.ToList().ForEach(x => Console.Write(x + " "));
            //Console.WriteLine();


            //var newdata = principalComponentAnalysis.Transform(data);

            //newdata.ToList().ForEach(x =>
            //{
            //    //x.ToList().ForEach(y => Console.Write(y + " "));
            //    Console.WriteLine();
            //});

            //principalComponentAnalysis.NumberOfOutputs = 1;

            //newdata = principalComponentAnalysis.Transform(data);

            //newdata.ToList().ForEach(x =>
            //{
            //    //x.ToList().ForEach(y => Console.Write(y + " "));
            //    Console.WriteLine();
            //});

            principalComponentAnalysis.ExplainedVariance = 1;

            var newdata = principalComponentAnalysis.Transform(data);

            Console.WriteLine(newdata[0].Count());

            var samples = Enumerable.Range(0,labels.Count()-1).Select(x => 
            {
                var sample = newdata[x].Select(y => (y >= 0 ? y == 0 ? 0 : 1 : -1).ToString()).ToList();
                sample.Add(labels[x]);

                return sample;
            }).ToList();

            var firstSamples = samples.Select(x => x.First()).ToList();

            //File.WriteAllLines("Datas/samolesPCA.txt", samples
            //    .Select(x => x
            //        .Aggregate((y, z) => y + " " + z))
            //    .ToArray());

            var position = samples.Count() * percentDataTraining / 100;

            Console.WriteLine($"position: {position}");

            TestSamples = samples.Skip(position).ToList();

            Samples = samples.Take(position).ToList();

            Attributes = Enumerable.Range(0, samples.First().Count() - 1).Select(x => x.ToString()).ToList();

            var q = 0;
            //newdata.ToList().ForEach(x =>
            //{
            //    //x.ToList().ForEach(y => Console.Write(y + " "));
            //    Console.WriteLine();
            //});
        }

        //public List<List<string>> GetTestSamples()
        //{
        //    TestSamples = File.ReadAllLines("Datas/test.arff.txt").Select(x =>
        //    {
        //        var sample = x.Split(' ').ToList();

        //        return sample;
        //    }).ToList();

        //    return TestSamples;
        //}
    }
}
