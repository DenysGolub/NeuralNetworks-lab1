using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_002
{
    internal class LeastSquares
    {
        double[,] squares_table = new double[0,0];
        public LeastSquares(double[,] squares_table)
        {
            this.squares_table = squares_table;
        }
        public double Slope { get; set; }
        public double Intercept { get; set; }
        public LeastSquares()
        {

        }
        //https://www.youtube.com/watch?v=P8hT5nDai6A
        public void GetLineCoefficients(out double slope, out double incercept)
        {
            var sums = CalculateSums();

            //y=mx+b
            //slope=m
            //incercept = b

            //m=(n*sum(xy)-sum(x)*sum(y))/(n*sum(x^2)-(sum(x))^2)
            double n = squares_table.GetLength(0);
            slope = (n * sums["xy"] - sums["x"] * sums["y"]) / (n * sums["x^2"] - Math.Pow(sums["x"],2));
            incercept = (sums["y"] - slope * sums["x"]) / n;

            Slope = slope; 
            Intercept = incercept;

        }

        private Dictionary<string,double> CalculateSums()
        {
            var array = ResizeArray(squares_table, squares_table.GetLength(0), squares_table.GetLength(1)+2);
            var sums = new Dictionary<string, double>()
            {
                {"x", 0 },
                {"y", 0 },
                {"xy", 0 },
                {"x^2",0 }
            };
            for (int row = 0; row < squares_table.GetLength(0); row++)
            {
                double x = array[row, 0]; sums["x"] += x;
                double y = array[row, 1]; sums["y"] += y;

                array[row, 2] = x * y; sums["xy"] += x * y;
                array[row, 3] = x * x; sums["x^2"] += x * x;

            }

            return sums;
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
    }
}
