using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApp1;
using WpfApp1.Controllers;
using System.IO;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /*public event EventHandler ChooseFileEventHandler;
        public event EventHandler LoadDataEventHandler;
        public event EventHandler SelectAttributesEventHandler;
        public event EventHandler BuildDecisionTreeEventHandler;
        public event EventHandler ViewDecisionTreeEventHandler;
        public event EventHandler TestDecisionTreeEventHandler;*/

        private DataController dataController = DataController.Instance;

        private DecisionTreeController decisionTreeController = DecisionTreeController.Instance;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChooseFile_Click(object sender, RoutedEventArgs e)
        {
            //ChooseFileEventHandler.Invoke(sender, e);
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Arff Data Files (*.arff)|*.arff|All Files (*.*)|*.*"
            };

            dlg.ShowDialog();

            //string filename = ;

            FileName.Text = dlg.FileName;
            DataController.pathFile = dlg.FileName;
            //Thread.Sleep(2000);
            //dlg.

            /*if (result == true)
            {
                // Open document 
                
                FileName.Text = filename;
            }*/
        }

        private void LoadData_Click(object sender, RoutedEventArgs e)
        {
            //LoadDataEventHandler.Invoke(sender, e);
            ListBoxLog.Items.Add("Load Starting");
            dataController.LoadSamples(int.Parse(PercentDataTraining.Text));
            ListBoxLog.Items.Add("Load Finished");
            /*dataController.Samples.ForEach(x =>
            {
                x.ForEach(y => Console.Write(y));
                Console.WriteLine();
            });*/
            /*dataController.Samples.ForEach(x =>
            {
                var sample = "";
                x.ForEach(y => sample += y);
                ListBoxLog.Items.Add(sample);
            });*/
        }

        private void SelectAttributes_Click(object sender, RoutedEventArgs e)
        {
            //SelectAttributesEventHandler.Invoke(sender, e);
            ListBoxLog.Items.Add("Select Attributes Starting");
            Task.Run(() =>
            {
                Dispatcher.Invoke(() => 
                {
                    dataController.ReduceAttribute(int.Parse(PercentDataTraining.Text));
                    ListBoxLog.Items.Add("Select Attributes Finished");
                });
            });
        }

        private void BuildDecisionTree_Click(object sender, RoutedEventArgs e)
        {
            var dateTimeStart = DateTime.Now;

            ListBoxLog.Items.Add($"{dateTimeStart} Build Starting");
            RichTextBoxLog.AppendText("Build Starting");

            Task.Run(() =>
            {
                Console.WriteLine("build 1");
                decisionTreeController.BuildDecisionTree(DecisionTreeController.DecisionTreeModel, dataController.Samples);
                Console.WriteLine("build 2");
                Dispatcher.Invoke(() =>
                {
                    var dateTimeEnd = DateTime.Now;

                    ListBoxLog.Items.Add($"{dateTimeEnd} Build Finished: {dateTimeEnd - dateTimeStart}");
                    RichTextBoxLog.AppendText("Build Finished");

                    ListBoxLog.SelectedIndex = ListBoxLog.Items.Count - 1;
                    ListBoxLog.ScrollIntoView(ListBoxLog.SelectedItem);
                });
                
            });

            //var program = new ConsoleApp1.Program();
            //Console.WriteLine(program.Run(dataController.Samples, dataController.TestSamples));
            //ListBoxLog.Items.Add(program.Run(dataController.Samples, dataController.TestSamples));
        }

        private void ViewDecisionTree_Click(object sender, RoutedEventArgs e)
        {
            var decisionTreeNodes = new List<List<string>>();
            //{
            //    new List<string>() { DecisionTreeController.DecisionTreeModel.Attribute }
            //};

            decisionTreeController.ViewDecisionTree(DecisionTreeController.DecisionTreeModel, decisionTreeNodes, DecisionTreeController.DecisionTreeModel, 0);

            decisionTreeNodes.ForEach(x =>
            {
                var decisionTreeNodeRow = x.Aggregate((y, z) => y + " " + z);
                ListBoxLog.Items.Add(decisionTreeNodeRow);
            });
        }

        private void TestDecisionTree_Click(object sender, RoutedEventArgs e)
        {
            //TestDecisionTreeEventHandler.Invoke(sender, e);
            var testSampels  = dataController.TestSamples.Select(x => x.ToArray()).ToArray();
            
            ListBoxLog.Items.Add($"Test Starting");

            Task.Run(() =>
            {
                decisionTreeController.TestDecisionTrees(DecisionTreeController.DecisionTreeModel, testSampels);
                
                Dispatcher.Invoke(() =>
                {
                    decisionTreeController
                        .ResultTest
                        .GroupBy(x => x)
                        .Select(x => new
                        {
                            a = x.Key,
                            b = x.Count()
                        })
                        .GroupBy(x => x.a.Split("->".ToArray(), StringSplitOptions.None).First())
                        .OrderBy(x => x.Key)
                        .ToList()
                        .ForEach(x =>
                        {
                            ListBoxLog.Items.Add(x.Key);

                            x.ToList().ForEach(y =>
                            {
                                ListBoxLog.Items.Add(y.a + " " + y.b);
                            });

                            ListBoxLog.Items.Add("");
                        });

                    /*dataController.TestSamples.ForEach(x =>
                    {
                        x.ForEach(y => Console.Write(y));
                        Console.WriteLine();
                    });

                    Console.WriteLine(dataController.Samples.Count());
                    Console.WriteLine(dataController.TestSamples.Count());
                    Console.WriteLine(DecisionTreeController.Node);
                    Console.WriteLine(decisionTreeController.ResultTest.Count(x => x.Contains("true")));*/

                    /*DecisionTreeController.Node = 0;
                    decisionTreeController.TestDecisionTrees(DecisionTreeController.DecisionTreeModel, dataController.TestSamples.Select(x => x.ToArray()).ToArray());
                    
                    Console.WriteLine(DecisionTreeController.Node);*/

                    ListBoxLog.Items.Add(DecisionTreeController.Node / (double)dataController.TestSamples.Count());

                    ListBoxLog.Items.Add("");

                    var dateTimeEnd = DateTime.Now;

                    //ListBoxLog.Items.Add($"{dataTimeStart} Build Starting");

                    ListBoxLog.Items.Add($"{dateTimeEnd} Test Finished");

                    ListBoxLog.SelectedIndex = ListBoxLog.Items.Count- 1;
                    ListBoxLog.ScrollIntoView(ListBoxLog.SelectedItem);
                });
            });

            //var program = new Program();
            //Console.WriteLine(program.Run(dataController.Samples, dataController.TestSamples));
        }
    }

}
