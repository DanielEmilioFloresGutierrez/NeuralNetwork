using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NNDemo
{
    public class Neuron
    {
        private double[] Inputs;
        public Neuron[] Synapsis;
        private double[] Weights;
        
        //public Neuron(double[] inputs)
        //{
        //    Inputs = (double[])inputs.Clone();
        //    Weights = new double[Inputs.Length+1];
        //    Random rd = new Random();
        //    for (int i=0;i<Weights.Length;i++)
        //    {
        //        Weights[i]=rd.NextDouble() * 10;
        //    }
           
        //}

        public Neuron(int inputs, Random rd)
        {
            Inputs = new double[inputs];
            Weights = new double[Inputs.Length + 1];//The extra input is a bias
            for (int i = 0; i < Weights.Length; i++)
            {
                if (rd.Next(2)>0)
                {
                    Weights[i] = rd.NextDouble();
                }
                else
                {
                    Weights[i] = rd.NextDouble()*(-1);
                }
               
            }
            
        }

        public void SetInputs(double[] inputs)
        {
            Inputs = inputs;
        }

        public double[] GetWeights()
        {
            return Weights;
        }

        public double GenerateOutput()
        {
            double sum=0;
            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i] * Weights[i];
            }
            sum -= Weights[Weights.Length-1];
            return Sigmoid(sum);
        }

        public void AdjustWeights(double error, double alpha, double beta)
        {
            for (int i = 0; i < Weights.Length - 1; i++)
            {
                double previousWeight = Weights[i];
                Weights[i] = Weights[i] + (alpha * error * Inputs[i]);
                Weights[i] += (beta * (Weights[i]-previousWeight));
            }
            Weights[Weights.Length - 1] -=alpha*error;
        }

        private double Sigmoid(double sum)
        {
            double output=1/(1+Math.Exp(-sum));
            return output;
        }
    }
}
