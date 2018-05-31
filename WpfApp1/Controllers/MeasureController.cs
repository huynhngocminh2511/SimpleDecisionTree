using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Controllers
{
    public class MeasureController
    {
        public static MeasureController Instance = new MeasureController();

        private List<List<string>> samples = DataController.Instance.Samples;

        private MeasureController()
        {

        }

        public double CalculEntropy(IEnumerable<int> numberEachLabels, double sumSamples)
        {
            return numberEachLabels.Sum(x => 
            {
                /*Console.WriteLine("CalculEntropy");
                Console.WriteLine(x);
                Console.WriteLine(-(x / sumSamples) * Math.Log(x / sumSamples, 2));
                Console.WriteLine(Math.Log(x / sumSamples, 2));*/
                return x == 0 ? 0 : -(x / sumSamples) * Math.Log(x / sumSamples, 2);
            });
        }

        //public double CalculInformationGain2(double entropy, int indexSample, IEnumerable<List<string>> samples)
        //{
        //    //var numberAttribute = DataController.Instance.Samples.GroupBy(x => x[indexSample]).Count();
        //    var attributes = new int[] { 0, 1 };

        //    //Enumerable.Range(0, numberAttribute);//.Select(y => int.Parse(y.Key)).ToList();
        //    //samples.GroupBy(x => x[indexSample]).Select(y => int.Parse(y.Key)).ToList();
        //    //new int[] { 0, 1 };

        //    var sampleAttributes = attributes
        //        .Select(y => samples
        //            .Where(z => z[indexSample]
        //                .Equals(y.ToString())).ToList())
        //        .ToList();

        //    /*if (sampleAttributes.Count() == 1)
        //    {
        //        var w = 0;
        //    }*/


        //    //var q = 0;

        //    //numberEachAttributes.ToList().ForEach(x => Console.WriteLine(x.Count()));

        //    /*var numberEachAttributesTest = attributes
        //            .Select(x => x
        //                .Where(z => z[256]
        //                    .Equals(.ToString()))
        //                .Count()).ToList();*/

        //    //var q = 0;


        //    /*var numberEachAttributes = sampleAttributes[0].ToList()
        //            .Select(y => samples
        //                .Where(z => z[indexSample]
        //                    .Equals(y.ToString()))
        //                .Count()).ToList();*/

        //    //var q = 0;

        //    //Console.WriteLine(indexSample);

        //    return entropy - attributes.Sum(x =>
        //    {
        //        //Console.WriteLine(x);

        //        //Console.WriteLine(1);

        //        var labels = samples.GroupBy(y => y.Last()).Select(z => int.Parse(z.Key)).ToList();
        //        //new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        //        //samples.GroupBy(x => x[indexSample]).Select(y => int.Parse(y.Key)).ToList();

        //        var numberEachAttributes = labels
        //            .Select(y => sampleAttributes[x]
        //                .Where(z => z.Last()
        //                    .Equals(y.ToString()))
        //                .Count()).ToList();

        //        //Console.WriteLine(2);

        //        //Console.WriteLine("123"+numberEachAttributes.Count());

        //        //numberEachAttributes.ForEach(y => Console.WriteLine(y));

        //        //Console.WriteLine(3);

        //        //var q = 0;
        //        //var w = 1 / q;

        //        //Console.WriteLine(CalculEntropy(numberEachAttributes, sampleAttributes[x].Count()));

        //        return CalculEntropy(numberEachAttributes, sampleAttributes[x].Count()) * sampleAttributes[x].Count() / samples.Count();

        //        //return 0;
        //    });
        //}

        public double CalculInformationGain(double entropy, int indexSample, IEnumerable<List<string>> samples)
        {
            //var numberAttribute = DataController.Instance.Samples.GroupBy(x => x[indexSample]).Count();
            //var attributes = new int[] { 0, 1 };

            //Enumerable.Range(0, numberAttribute);//.Select(y => int.Parse(y.Key)).ToList();
            //samples.GroupBy(x => x[indexSample]).Select(y => int.Parse(y.Key)).ToList();
            //new int[] { 0, 1 };

            //var sampleAttributes = attributes
            //    .Select(y => samples
            //        .Where(z => z[indexSample]
            //            .Equals(y.ToString())).ToList())
            //    .ToList();

            var sampleAttributes = samples.GroupBy(x => x[indexSample]);
                //attributes
                //.Select(y => samples
                //    .Where(z => z[indexSample]
                //        .Equals(y.ToString())).ToList())
                //.ToList();

            /*if (sampleAttributes.Count() == 1)
            {
                var w = 0;
            }*/


            //var q = 0;

            //numberEachAttributes.ToList().ForEach(x => Console.WriteLine(x.Count()));

            /*var numberEachAttributesTest = attributes
                    .Select(x => x
                        .Where(z => z[256]
                            .Equals(.ToString()))
                        .Count()).ToList();*/

            //var q = 0;


            /*var numberEachAttributes = sampleAttributes[0].ToList()
                    .Select(y => samples
                        .Where(z => z[indexSample]
                            .Equals(y.ToString()))
                        .Count()).ToList();*/

            //var q = 0;

            //Console.WriteLine(indexSample);

            return entropy - sampleAttributes.Sum(x =>
            {
                //Console.WriteLine(x);

                //Console.WriteLine(1);

                //var labels = samples.GroupBy(y => y.Last()).Select(z => int.Parse(z.Key)).ToList();
                var labels = samples.GroupBy(y => y.Last()).Select(z => z.Key).ToList();
                //new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

                //samples.GroupBy(x => x[indexSample]).Select(y => int.Parse(y.Key)).ToList();

                var numberEachAttributes = labels
                    .Select(y => x
                        .Where(z => z.Last()
                            .Equals(y))
                        .Count()).ToList();

                //Console.WriteLine(2);

                //Console.WriteLine("123"+numberEachAttributes.Count());

                //numberEachAttributes.ForEach(y => Console.WriteLine(y));

                //Console.WriteLine(3);

                //var q = 0;
                //var w = 1 / q;

                //Console.WriteLine(CalculEntropy(numberEachAttributes, sampleAttributes[x].Count()));

                return CalculEntropy(numberEachAttributes, x.Count()) * x.Count() / samples.Count();

                //return 0;
            });
        }
    }
}
