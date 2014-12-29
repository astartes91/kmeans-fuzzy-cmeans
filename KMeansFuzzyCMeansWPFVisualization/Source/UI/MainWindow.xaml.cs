using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;

namespace KMeansFuzzyCMeansWPFVisualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BackgroundWorker BackgroundWorker;

        public MainWindow()
        {
            InitializeComponent();
            BackgroundWorker = (BackgroundWorker)FindResource("BackgroundWorker");
        }

        /// <summary>
        /// Обработчик кнопки "Generate Data"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateDataButton_Click(object sender, RoutedEventArgs e)
        {
            var plotModel = this.plot.Model;
            //Генерация случайных точек
            HashSet<List<double>> points = AlgorithmsUtils.GenerateDataPoints(2);
            foreach (List<double> point in points)
            {
                //отображение точек на графике
                PointAnnotation annotation = new PointAnnotation { Shape = MarkerType.Circle, X = point[0], Y = point[1] };
                plotModel.Annotations.Add(annotation);
            }

           plot.InvalidatePlot();
        }

        /// <summary>
        /// Обработчик кнопки "Run K-Means"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunKMeansButton_Click(object sender, RoutedEventArgs e)
        {
            #region Код мультизадачности (пока не используется)

            /*Args args = new Args();
            args.label = this.iterationLabel;
            args.number = int.Parse(this.NumberOfClustersTextBox.Text);
            args.plot = plot;
            BackgroundWorker.RunWorkerAsync(args);*/
            /*Task.Factory.StartNew(() => Algorithm.DoClusteringByKMeans(plot, this.iterationLabel,
                    int.Parse(NumberOfClustersTextBox.Text)));*/

            #endregion Код мультизадачности (пока не используется)

            int numberOfClusters;
            bool parsingResult = int.TryParse(this.NumberOfClustersTextBox.Text, out numberOfClusters);

            if (parsingResult && numberOfClusters > 1)
            {
                KMeansAlgorithm.DoClusteringByKMeans(plot, this.iterationLabel, numberOfClusters);
            }           
        }

        #region Код мультизадачности (пока не используется)

        /*private void BackgroundWorker_OnDoWork(object sender, DoWorkEventArgs e)
        {
            Args args = (Args) e.Argument;
            Algorithm.DoClusteringByKMeans(args.plot, args.label,
                args.number);
        }*/

        #endregion Код мультизадачности (пока не используется)

        /// <summary>
        /// Обработчик кнопки "Run Fuzzy С-Means"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunFuzzyСMeansButton_OnClick(object sender, RoutedEventArgs e)
        {
            int numberOfClusters;
            bool numberOfClustersTextBoxParsingResult = int.TryParse(this.NumberOfClustersTextBox.Text, out numberOfClusters);

            double fuzzificationParameter;
            bool fuzzificationParameterTextBoxParsingResult = double.TryParse(this.FuzzificationParameterTextBox.Text, out fuzzificationParameter);

            if (numberOfClustersTextBoxParsingResult && fuzzificationParameterTextBoxParsingResult && numberOfClusters > 1 && fuzzificationParameter > 1)
            {
                FuzzyCMeansAlgorithm.DoClusteringByFuzzyCMeans(plot, this.iterationLabel, numberOfClusters, fuzzificationParameter);
            }           
        }


        /// <summary>
        /// Обработчик события загрузки окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> </param>
        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            //Создание графика
            var plotModel = new PlotModel();
            var linearAxisX = new LinearAxis { Position = AxisPosition.Bottom };
            linearAxisX.Title = "X Axis";
            plotModel.Axes.Add(linearAxisX);

            var linearAxisY = new LinearAxis();
            linearAxisY.Title = "Y Axis";
            plotModel.Axes.Add(linearAxisY);

            this.plot.Model = plotModel;
        }
    }
}