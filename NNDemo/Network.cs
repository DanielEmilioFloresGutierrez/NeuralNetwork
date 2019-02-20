using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NNDemo
{
    public class Network
    {
        private bool IsReady=false;
        private double ErrorMargin;
        private double CurrentError;
        private double[][] DataInput;//Contains the inputs for training
        private double[] DataOutput;//Contains the outputs for training
        private int Epochs;
        private double Alpha;
        private double Beta;
        private double[] TempInputs;
        private double[][][] Weights;
        private Neuron[][] net;
        private Neuron OutputNeuron;
        private Random rd = new Random();
        
        //Function ready
        public Network(double[][] dataTable, int layers=1, double alpha=.5, double beta=.1, int epochs=1000, double errorMargin = 0.01)
        {
            DataInput = dataTable.GetColumns(0,dataTable[0].Length-2);
            DataOutput = dataTable.GetColumn(dataTable[0].Length-1);
            Epochs = epochs;
            ErrorMargin = errorMargin;
            Alpha = alpha;
            Beta = beta;
            Weights = new double[layers][][];
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = new double[DataInput[0].Length][];
            }
            TempInputs = new double[DataInput[0].Length];
            InitializeNetwork(layers,DataInput[0].Length);
            LinkNeurons();
        }
        
        //Function ready
        private void InitializeNetwork(int layers,int neurons)
        {
            net = new Neuron[layers][];
            for (int i = 0; i < net.Length; i++)
            {
                net[i] = new Neuron[neurons];
                for (int j = 0; j < net[i].Length; j++)
                {
                    net[i][j] = new Neuron(DataInput[0].Length,rd);
                    Weights[i][j] = (double[])net[i][j].GetWeights().Clone();
                }
            }
            OutputNeuron = new Neuron(DataInput[0].Length,rd);
        }

        //To do
        public void Train()
        {
            int iteration = 0;
            do
            {

                //For every sample in the DataInput
                for (int InputRow = 0; InputRow < DataInput.Length; InputRow++)
                {
                    
                    //Sets the inputs of the first layer to be the values from the DataInput
                    for (int FirstLayerNeuron = 0; FirstLayerNeuron < net[0].Length; FirstLayerNeuron++)
                    {
                        net[0][FirstLayerNeuron].SetInputs(DataInput[InputRow]);
                    }

                    //Sets the inputs of the next layers to be the output of the previous ones
                    for (int layer = 1; layer < net.Length; layer++)
                    {
                        //Update the TempInputs
                        for (int output = 0; output < net[0].Length; output++)
                        {
                            TempInputs[output] = net[layer - 1][output].GenerateOutput();
                        }

                        //Charge the TempInputs to every neuron of the next layer
                        for (int neuron = 0; neuron < net[layer].Length; neuron++)
                        {
                            net[layer][neuron].SetInputs(TempInputs);
                        }
                    }

                    //Set the inputs of the OutputNeuron
                    for (int output = 0; output < net[0].Length; output++)
                    {
                        TempInputs[output] = net[net.Length - 1][output].GenerateOutput();
                    }
                    OutputNeuron.SetInputs(TempInputs);
                    

                    //Calculates the error of the sample
                    double sampleError = CalculateError(OutputNeuron.GenerateOutput(), InputRow, DataOutput);
                    
                    CurrentError += Math.Pow(sampleError,2);

                    //Adjusts the weights
                    for (int layer = 0; layer < net.Length; layer++)
                    {
                        for (int neuron = 0; neuron < net[layer].Length; neuron++)
                        {
                            net[layer][neuron].AdjustWeights(sampleError,Alpha,Beta);
                        }
                    }
                    OutputNeuron.AdjustWeights(sampleError,Alpha,Beta);
                    
                }

                CurrentError/=DataInput.Length;
                Console.WriteLine(CurrentError);//**********************************
                IsReady = CurrentError <= ErrorMargin;
                iteration++;
            }
            while (!IsReady && iteration < Epochs);
            if (IsReady)
            {
                Console.WriteLine($"\n\nTraining succeded: Error {CurrentError}");
            }
            else
            {
                Console.WriteLine($"\n\nEpochs concluded: Error {CurrentError}");
            }
        }

        public double Predict(double input1, double input2)
        {
            TempInputs = new double[] { input1,input2 };

            //Sets the inputs of the first layer to be the values from the DataInput
            for (int FirstLayerNeuron = 0; FirstLayerNeuron < net[0].Length; FirstLayerNeuron++)
            {
                net[0][FirstLayerNeuron].SetInputs(TempInputs);
            }

            //Sets the inputs of the next layers to be the output of the previous ones
            for (int layer = 1; layer < net.Length; layer++)
            {
                //Update the TempInputs
                for (int output = 0; output < net[0].Length; output++)
                {
                    TempInputs[output] = net[layer - 1][output].GenerateOutput();
                }

                //Charge the TempInputs to every neuron of the next layer
                for (int neuron = 0; neuron < net[layer].Length; neuron++)
                {
                    net[layer][neuron].SetInputs(TempInputs);
                }
            }

            //Set the inputs of the OutputNeuron
            for (int output = 0; output < net[0].Length; output++)
            {
                TempInputs[output] = net[net.Length - 1][output].GenerateOutput();
            }
            OutputNeuron.SetInputs(TempInputs);
            return OutputNeuron.GenerateOutput();
           
        }

        //Function ready
        public void PrintWeights()
        {
            for (int layer = 0; layer < Weights.Length; layer++)
            {
                for (int neuron = 0; neuron < Weights[layer].Length; neuron++)
                {
                    for (int w = 0; w < Weights[layer][neuron].Length; w++)
                    {
                        Console.Write(Weights[layer][neuron][w]+"\t");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\n");
            }
        }

        //Function ready
        private void LinkNeurons()
        {
            for (int i = net.Length-1; i > 0; i--)
            {
                for (int j = 0; j < net[i].Length; j++)
                {
                    net[i][j].Synapsis = net.GetColumn(i-1);
                }
            }
        }
        
        //Function ready
        private double CalculateError(double result, int searchRow, double[]expectedOutputs)
        {
            return (expectedOutputs[searchRow]) - result;
        }

    }
}
