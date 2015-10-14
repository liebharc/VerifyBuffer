using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeSnippets
{
    public class DoubleArrayCreation
    {
        public double[][] ScaleArrays(double[][] arrays, double scalingFactor)
        {
            var result = new double[arrays.Length][];
            for (int i = 0; i < arrays.Length; i++)
            {
                result[i] = ScaleArray(arrays[i], scalingFactor);
            }

            return result;
        }

        public double[] ScaleArray(double[] array, double scalingFactor)
        {
            var result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i] * scalingFactor;
            return result;
        }

        private const int Zero = 0;

        private static readonly double[] Empty = new double[Zero];

        private static readonly double[] Empty2 = new double[Zero2];

        private const int Zero2 = 0;

        public void EmptyArray()
        {
            const int Two = 2;
            var result = new double[Two];
        }
    }
}
