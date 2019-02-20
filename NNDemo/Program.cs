using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Network NN = new Network(TrainingData,layers:2,epochs:1000000,alpha:.2,beta:0,errorMargin:0.01);
            TrainingData.PrintMatrix();
            NN.Train();
            NN.PrintWeights();
            double[] input1 = { 0, 1 };
            double[] input2 = { 0, 1 };
            for (int i = 0; i < input1.Length; i++)
            {
                for (int j = 0; j < input2.Length; j++)
                {
                    Console.WriteLine($"For {input1[i]} and {input2[j]}: " + NN.Predict(input1[i], input2[j]));

                }
            }
            Console.ReadLine();
        }

        public static double[][] TrainingData =
        {
            new double[]{0,0,0 },
            new double[]{0,1,1 },
            new double[]{1,0,1 },
            new double[]{1,1,0 },
        };



        public static double Denormalize(double value, double min, double max)
        {
            return value * (max - min) + min;
        }

        
    }
}
