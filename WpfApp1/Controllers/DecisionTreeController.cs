using WpfApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Controllers
{
    public class DecisionTreeController
    {
        private List<List<string>> samples = DataController.Instance.Samples;

        private MeasureController measureController = MeasureController.Instance;

        private DataController dataController = DataController.Instance;

        public static DecisionTreeController Instance = new DecisionTreeController();

        public static DecisionTreeModel DecisionTreeModel = new DecisionTreeModel();

        public static int Node = 0;

        public List<String> ResultTest = new List<string>();

        private DecisionTreeController()
        {

        }

        public void BuildDecisionTree(DecisionTreeModel decisionTreeModel, List<List<string>> samples)
        {
            var q = samples.GroupBy(x => x.Last()).Select(y => y.Count());

            if (q.Count() == 1)//last leaf
            {
                decisionTreeModel.Attribute = samples[0].Last();
                //Console.WriteLine($"bulid result {samples[0].Last()}");
                return;
            }

            var entropy = measureController.CalculEntropy(q, samples.Count());

            var index = 0;
            var max = 0d;
            var attributesMax = 0;

            dataController.Attributes.ForEach(x =>
            {
                var informationGain = measureController.CalculInformationGain(entropy, int.Parse(x), samples);

                if (informationGain > max)
                {
                    max = informationGain;
                    attributesMax = index;
                }

                index++;
            });//find max gain information

            decisionTreeModel.Attribute = attributesMax.ToString();

            var samplesGroupBy = samples.GroupBy(x => x[attributesMax]);

            Enumerable.Range(0, samplesGroupBy.Count());

            List<Predicate<string>> predicates = samplesGroupBy.Select(x =>
            {
                Predicate<string> w2 = (string attribute) => attribute.Equals(x.Key);

                //Predicate<string> w2 = (string attribute) => 
                //{
                //    double.TryParse(attribute, out double number);
                //    if (number != 0)
                //    {
                //        double.TryParse(attribute, out double keyNumber);
                //        return Math.Abs(number - keyNumber) <= 0.01d;
                //    }

                //    return attribute.Equals(x.Key);
                //};

                return w2;
            }).ToList();

            decisionTreeModel.Predicates = predicates;

            var r = samplesGroupBy.Select(x =>
            {
                return x.ToList();
            }).ToList();

            var decisionTreeModels = new List<DecisionTreeModel>();

            samplesGroupBy.ToList().ForEach(x =>
            {
                var asdasd = x.ToList();
                var subDecisionTreeModel = new DecisionTreeModel();
                decisionTreeModels.Add(subDecisionTreeModel);
                BuildDecisionTree(subDecisionTreeModel, x.ToList());
            });

            decisionTreeModel.DecisionTreeModels = decisionTreeModels;
        }

        //test multi sample
        public void TestDecisionTrees(DecisionTreeModel decisionTreeModel, params string[][] sample)
        {
            ResultTest.Clear();
            Node = 0;
            sample.ToList().ForEach(x => TestDecisionTree(decisionTreeModel, x));
        }

        //test single sample
        public void TestDecisionTree(DecisionTreeModel decisionTreeModel, string[] sample)
        {
            Console.WriteLine($"1 Test {sample.Count()}");
            Console.WriteLine($"2 Test {sample.Count()}");

            if (decisionTreeModel.DecisionTreeModels == null)
            {
                Console.WriteLine(decisionTreeModel.Attribute.Equals(sample.Last())
                    ? $"true {sample.Last()} -> {decisionTreeModel.Attribute}"
                    : $"false {sample.Last()} -> {decisionTreeModel.Attribute}");

                if (decisionTreeModel.Attribute.Equals(sample.Last()))
                {
                    Node++;
                }

                var nstr = decisionTreeModel.Attribute.Equals(sample.Last())
                    ? $"{sample.Last()} -> {decisionTreeModel.Attribute}"
                    : $"{sample.Last()} -> {decisionTreeModel.Attribute}";

                ResultTest.Add(nstr);

                return;
            }

            var str = sample[int.Parse(decisionTreeModel.Attribute)];

            var index = 0;

            decisionTreeModel.Predicates.ForEach(x =>
            {
                if (x(str))
                {
                    TestDecisionTree(decisionTreeModel.DecisionTreeModels[index], sample);
                }

                index++;
            });
        }

        public void ViewDecisionTree(DecisionTreeModel decisionTreeModel, List<List<string>> decisionTreeNodes, DecisionTreeModel fatherDecisionTreeModel, int deep)
        {
            //var queue = new Queue<DecisionTreeModel>();
            //queue.Enqueue(decisionTreeModel);
            ////var newdeep = 0;

            //while (queue.Any())
            //{
            //    var CurrentDecisionTreeModel = queue.Dequeue();

            //    var decisionTreeNode = CurrentDecisionTreeModel.DecisionTreeModels
            //    .Select(x => x.Attribute)
            //    .ToList();

            //    if (decisionTreeNodes[deep] == null)
            //    {
            //        decisionTreeNodes[deep].Add(decisionTreeNode);
            //    }

            //    //decisionTreeNodes[deep] ?? new List<string>();//.Add(decisionTreeNode);
            //}
            //if (decisionTreeModel.DecisionTreeModels == null) return;



            /*decisionTreeModel.DecisionTreeModels.ForEach(x =>
            {
                
            });*/

            //if (decisionTreeNodes[deep] == null)
            //{
            //    decisionTreeNodes[deep] = new List<string>();
            //}
            //decisionTreeNodes[deep] ?? decisionTreeNodes.Add(new List<string>());

            /*try
            {
                var q = decisionTreeNodes[deep]?? decisionTreeNodes.Add(new List<string>());
            }
            catch (ArgumentOutOfRangeException argumentOutOfRangeException)
            {
                decisionTreeNodes.Add(new List<string>());
            }*/

            if (decisionTreeNodes.Count()<=deep)
            {
                decisionTreeNodes.Add(new List<string>());
            }

            decisionTreeNodes[deep].Add($"{deep}:{fatherDecisionTreeModel.Attribute}:{decisionTreeModel.Attribute}");

            if (decisionTreeModel.DecisionTreeModels == null)
            {
                return;
            }

            decisionTreeModel.DecisionTreeModels.ForEach(x =>
            {
                ViewDecisionTree(x, decisionTreeNodes, decisionTreeModel, deep + 1);
            });
        }
    }
}
