using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.GL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Task_001
{
    internal class Perceptron
    {
        double _c = 0.15;                                                                                                                                                                                                                           
        double[] W = [0.8,
                      0.4,
                      -1.2];

        public double LearningRate { get
            {
                return _c;
            }
            set
            {
                _c = value;
            }
        }

        public double[] Weights => W;
        private int sign(double net)
        {
            return net >= 0 ? 1 : -1;
        }
       
        /// <summary>
        /// trainingMode = true - is automatic, if false = manual
        ///
        /// </summary>
        /// <param name="train_sample"></param>
        /// <param name="trainingMode"></param>
        /// <returns></returns>
        public void Train(double[,] train_sample, TextBox logs, 
            out bool isLinearSeparable, 
            out bool hasError, 
            bool trainingMode=true)
        {
            logs.Clear();

            logs.ScrollToEnd();
            hasError = true;

            if(trainingMode)
            {
                int maxEpoch = 10000;
                int epoch = 0;
                while (hasError)
                {
                    epoch++;
                   
                    hasError = false; 

                    for (int row = 0; row < train_sample.GetLength(0); row++)
                    {
                        double net = 0;

                        for (int col = 0; col < train_sample.GetLength(1) - 1; col++)
                        {
                            net += train_sample[row, col] * W[col];
                        }
                        net += Math.Round(W[2], 3);

                        double expectedOutput = train_sample[row, train_sample.GetLength(1) - 1];
                        double fnet = sign(net);  

                        if (fnet != expectedOutput)
                        {
                            hasError = true;

                            for (int col = 0; col < train_sample.GetLength(1) - 1; col++)
                            {
                                double deltaW = _c * (expectedOutput - fnet) * train_sample[row, col];
                                W[col] += deltaW;
                                W[col] = Math.Round(W[col], 3);
                            }

                            W[2] += _c * (expectedOutput - fnet);
                            W[2] = Math.Round(W[2], 3);

                        }
                    }
                    if(epoch>= maxEpoch)
                    {
                        logs.Text = "Вибірка не є лінійно роздільною!\n";
                        isLinearSeparable = false;
                        return;
                    }
                }
                logs.Text += ($"Навчання було завершено на {epoch} епохі.\n");
                logs.Text += $"Ваги: W1={W[0]}, W2={W[1]}, W3={W[2]}\n";

            }
            else
            {
                hasError = false;
                for (int row = 0; row < train_sample.GetLength(0); row++)
                {
                    logs.Text += $"Ваги: W1={W[0]}, W2={W[1]}, W3={W[2]}\n";

                    double net = 0;
                    string net_log = "net=";

                    for (int col = 0; col < train_sample.GetLength(1) - 1; col++)
                    {
                        net += train_sample[row, col] * W[col];
                        net_log += $"{train_sample[row, col]} * {W[col]}+";
                    }
                    
                    net += Math.Round(W[2], 3);
                    net_log += $"{Math.Round(W[2], 3)}";
                    logs.Text += $"{net_log}={net}\n";
                    double expectedOutput = train_sample[row, train_sample.GetLength(1) - 1];
                    double fnet = sign(net);
                    logs.Text += $"Очікуваний результат: {expectedOutput}\nf(net)={fnet}\n";

                    if (fnet != expectedOutput)
                    {
                        logs.Text += "Відповіді не збігаються. Відбувається корегування ваг.\n";
                        hasError = true;

                        for (int col = 0; col < train_sample.GetLength(1) - 1; col++)
                        {
                            double deltaW = _c * (expectedOutput - fnet) * train_sample[row, col];
                            W[col] += deltaW;
                            W[col] = Math.Round(W[col], 3);
                            logs.Text += $"ΔW{col + 1}={deltaW}\n";
                            logs.Text += $"W{col + 1}+ΔW={W[col]}\n";
                        }

                        W[2] += _c * (expectedOutput - fnet);
                        W[2] = Math.Round(W[2], 3);
                        logs.Text += $"ΔW3\n";
                        logs.Text += $"W3+ΔW={W[2]}\n";
                    }
                    logs.Text += "----------\n";

                }

            }
            isLinearSeparable = true;
        }

        public int Predict(double[]sample)
        {
            return sign(sample[0] * W[0] + sample[1] * W[1] + W[2]);
        }
    }
}
