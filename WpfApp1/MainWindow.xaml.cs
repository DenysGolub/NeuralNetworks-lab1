using Gu.Wpf.DataGrid2D;
using ScottPlot;
using System.DirectoryServices;
using System.Drawing;
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
using Task_002;
using Color = ScottPlot.Color;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LeastSquares leastSquares = new LeastSquares();
        private double[,] input = new double[,]
        {
            {0.19, 1.21},
            {0.21, 1.30},
            {0.31, 1.28 },
            {0.41, 1.39 },
            {0.49, 1.65 },
            {0.60, 1.73 },
            {0.81, 1.72 },
            {0.91, 2.04 },
            {1.01, 2.15 }
        };
        public MainWindow()
        {
            InitializeComponent();

            squaresInput.SetArray2D(input);
            squaresInput.SetColumnHeadersSource(new List<string> { "x", "y" }.ToArray());
            PlotPoints();
        }

        private void PlotPoints()
        {
            WpfPlot1.Plot.Clear();
            for (int i = 0; i < input.GetLength(0); i++)
            {
                var point = WpfPlot1.Plot.Add.Scatter(input[i, 0], input[i, 1]);
                point.MarkerSize = 15;
                point.MarkerColor = Color.FromHex("#A020F0");
            }
        }

        private void solution_button_Click(object sender, RoutedEventArgs e)
        {
            leastSquares = new LeastSquares(input);
            leastSquares.GetLineCoefficients(out double slope, out double intercept);
            PlotPoints();
            PlotLine(slope, intercept);
            output.Content = $"m(нахил)={slope}\nb(перетин)={intercept}";

            lineEquation.Content = $"y={Math.Round(slope,3)}*x+{Math.Round(intercept,3)}";
        }


        private void PlotLine(double m, double b)
        {
            GetMinAndMax(out double minX, out double maxX, out double minY, out double maxY);

            // Обчислюємо x2 відповідно до рівняння прямої: x2 = -(w1 / w2) * x1 - (w3 / w2)
            List<double> x = new List<double>();
            List<double> y = new List<double>();
            for (double i = minX-1; i <= maxX+1; i+=0.001)
            {
                x.Add(i);
                y.Add(m * i + b);

            }

            // Побудуємо графік
            var line = WpfPlot1.Plot.Add.ScatterLine(x.ToArray(), y.ToArray());
            line.LineWidth = 5;
            WpfPlot1.Plot.XLabel("x");
            WpfPlot1.Plot.YLabel("y");

            // Оновлюємо графік
            WpfPlot1.Refresh();
        }

        private void GetMinAndMax(out double minX, out double maxX,
   out double minY, out double maxY)
        {
            minX = double.MaxValue;
            maxX = double.MinValue;
            minY = double.MaxValue;
            maxY = double.MinValue;

            for (int row = 0; row < input.GetLength(0); row++)
            {
                double x1 = input[row, 0];

                if (x1 < minX)
                {
                    minX = x1;
                }

                if (x1 > maxX)
                {
                    maxX = x1;
                }
            }

            for (int row = 0; row < input.GetLength(0); row++)
            {
                double x2 = input[row, 1];

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

        private void clear_input_Click(object sender, RoutedEventArgs e)
        {
            input = ResizeArray(input, 0, 2);
            squaresInput.SetColumnHeadersSource(new List<string>() { "x", "y" }.ToArray());
            squaresInput.SetArray2D(input);
        }

        private void add_input_Click(object sender, RoutedEventArgs e)
        {
            input = ResizeArray(input, input.GetLength(0) + 1, input.GetLength(1));
            squaresInput.SetArray2D(input);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var k = leastSquares.Slope;
            var b = leastSquares.Intercept;

            var y = Math.Round(k, 3) * Math.Round(double.Parse(x_input.Text), 3) + b;
            y_out.Content = $"y={Math.Round(y,3)}";
            var line = WpfPlot1.Plot.Add.ScatterLine(new double[] { double.Parse(x_input.Text) }, new double[] { y });
            line.Color = Color.FromHex("#ff0000");
            line.MarkerSize = 25;

            WpfPlot1.Refresh();


        }
    }
}