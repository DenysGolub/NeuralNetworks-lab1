using Gu.Wpf.DataGrid2D;
using OpenTK.Windowing.GraphicsLibraryFramework;
using ScottPlot;
using ScottPlot.WPF;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Task_001;
using Color = ScottPlot.Color;
using Window = System.Windows.Window;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Perceptron nn = new Perceptron();
        private double[,] training_data =
        {
            {1,1,1 },
            {9.4, 6.4, -1 },
            {2.5, 2.1, 1 },
            {8, 7.7, -1 },
            {0.5, 2.2,1},
            {7.9, 8.4, -1 },
            {7,7,-1 },
            {2.8, 0.8, 1},
            {1.2, 3,1 },
            {7.8,6.1,-1 }
        };


        //public string[] ColumnHeaders => new string[] { "x1", "x2", "Вихід" };
        double[,] input = new double[5,3];
        public MainWindow()
        {
            InitializeComponent();
  
            
            trainingData.SetArray2D(training_data);
            trainingData.SetColumnHeadersSource(new List<string>() { "x1", "x2", "Вихід" }.ToArray());

            //weightsData.ItemsSource = nn.Weights;

        }

        

        private void add_training_input_Click(object sender, RoutedEventArgs e)
        {
            training_data = ResizeArray(training_data, training_data.GetLength(0)+1, training_data.GetLength(1));
            trainingData.SetArray2D(training_data);
        }

        T[,] ResizeArray<T>(T[,] original, int rows, int cols)
        {
            var newArray = new T[rows, cols];
            int minRows = Math.Min(rows, original.GetLength(0));
            int minCols = Math.Min(cols, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, j];
            return newArray;
        }

        private void start_train_bt_Click(object sender, RoutedEventArgs e)
        {
            nn.LearningRate = double.Parse(learningRate.Text);
            nn.Weights[0] = double.Parse(W0_input.Text);
            nn.Weights[1] = double.Parse(W1_input.Text);
            nn.Weights[2] = double.Parse(W2_input.Text);
            nn.Train(training_data, Logs, out bool lin_sep, out bool error);
            PlotPoints();

            PlotLine(nn.Weights);

            W0_input.Text = nn.Weights[0].ToString();
            W1_input.Text = nn.Weights[1].ToString();
            W2_input.Text = nn.Weights[2].ToString();

            W1.Text = nn.Weights[0].ToString();
            W2.Text = nn.Weights[1].ToString();
            W3.Text = nn.Weights[2].ToString();

        }

        private void PlotPoints()
        {
            WpfPlot1.Plot.Clear();


            for (int i = 0; i < training_data.GetLength(0); i++)
            {
                var point = WpfPlot1.Plot.Add.Scatter(training_data[i, 0], training_data[i, 1]);
                point.MarkerSize = 25;
               
                if (training_data[i, training_data.GetLength(1)-1] == 1)
                {
                    point.MarkerShape = MarkerShape.FilledCircle;
                    point.MarkerFillColor = Color.FromHex("#89CFF0");
                }
                else
                {
                    point.MarkerShape = MarkerShape.FilledTriangleUp;
                    point.MarkerFillColor = Color.FromHex("#C70039");

                }
            }

        }

        private void GetMinAndMax(out double minX, out double maxX,
     out double minY, out double maxY)
        {
            minX = double.MaxValue;
            maxX = double.MinValue;
            minY = double.MaxValue;
            maxY = double.MinValue;

            for (int row = 0; row < training_data.GetLength(0); row++)
            {
                double x1 = training_data[row, 0];

                if (x1 < minX)
                {
                    minX = x1;
                }

                if (x1 > maxX)
                {
                    maxX = x1;
                }
            }

            for (int row = 0; row < training_data.GetLength(0); row++)
            {
                double x2 = training_data[row, 1];

                if (x2 < minY)
                {
                    minY = x2;
                }

                if (x2 > maxY)
                {
                    maxY = x2;
                }
            }
        }


        private void PlotLine(double[]W)
        {
            GetMinAndMax(out double minX, out double maxX, out double minY, out double maxY);
            WpfPlot1.Plot.Axes.SetLimitsX(minX-2, maxX+2);
            WpfPlot1.Plot.Axes.SetLimitsY(minY-2, maxY+2);
            // Обчислюємо x2 відповідно до рівняння прямої: x2 = -(w1 / w2) * x1 - (w3 / w2)
            List<double> x1 = new List<double>();
            List<double> x2 = new List<double>();
            for (double i = minX; i <= maxX+2;  i++)
            {
                x1.Add(i);
                x2.Add(-(W[0] / W[1]) * i - (W[2] / W[1]));
            }

            // Побудуємо графік
            var line = WpfPlot1.Plot.Add.ScatterLine(x1.ToArray(), x2.ToArray());
            line.LineWidth = 5;
            WpfPlot1.Plot.XLabel("x1");
            WpfPlot1.Plot.YLabel("x2");

            // Оновлюємо графік
            WpfPlot1.Refresh();
        }

      
        private void nextIteration_Click(object sender, RoutedEventArgs e)
        {
            nn.LearningRate = double.Parse(learningRate.Text);
            nn.Weights[0] = double.Parse(W0_input.Text);
            nn.Weights[1] = double.Parse(W1_input.Text);
            nn.Weights[2] = double.Parse(W2_input.Text);

            nn.Train(training_data, Logs,out bool lin_sep, out bool error, false);
            PlotPoints();

            PlotLine(nn.Weights);

            W0_input.Text = nn.Weights[0].ToString();
            W1_input.Text = nn.Weights[1].ToString();
            W2_input.Text = nn.Weights[2].ToString();
            if (!error)
            {
                Logs.Text += $"НАВЧАННЯ ЗАВЕРШЕНО!";
            }


            W1.Text = nn.Weights[0].ToString();
            W2.Text = nn.Weights[1].ToString();
            W3.Text = nn.Weights[2].ToString();
        }

        private void ClassificationButton_Click(object sender, RoutedEventArgs e)
        {

            int predicted = nn.Predict(new double[] { double.Parse(x1_input.Text), double.Parse(x2_input.Text) });
            outputTB.Text = predicted.ToString();
        }

        private async void timeIteration_Click(object sender, RoutedEventArgs e)
        {
            nn.LearningRate = double.Parse(learningRate.Text);
            nn.Weights[0] = double.Parse(W0_input.Text);
            nn.Weights[1] = double.Parse(W1_input.Text);
            nn.Weights[2] = double.Parse(W2_input.Text);
            bool error = true;
            bool lin_sep = true;
            int epoch = 0;
            do
            {
                await Task.Delay(int.Parse(time.Text)*1000);

                nn.Train(training_data,Logs, out lin_sep, out error, false);
                PlotPoints();

                PlotLine(nn.Weights);
                W0_input.Text = nn.Weights[0].ToString();
                W1_input.Text = nn.Weights[1].ToString();
                W2_input.Text = nn.Weights[2].ToString();

                W1.Text = nn.Weights[0].ToString();
                W2.Text = nn.Weights[1].ToString();
                W3.Text = nn.Weights[2].ToString();
                epoch++;
            }
            while (error && lin_sep);

            if(!error)
            {
                Logs.Text += $"НАВЧАННЯ ЗАВЕРШЕНО! ПЕРЦЕПТРОН ТРЕНУВАВСЯ ПРОТЯГОМ {epoch} ЕПОХ!";
            }
            
        }

        private void clear_training_input_Click(object sender, RoutedEventArgs e)
        {
            training_data = ResizeArray(training_data, 0, 3);
            trainingData.SetColumnHeadersSource(new List<string>() { "x1", "x2", "Вихід" }.ToArray());
            trainingData.SetArray2D(training_data);
        }
    }
}