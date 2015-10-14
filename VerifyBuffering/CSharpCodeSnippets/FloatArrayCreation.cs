using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeSnippets
{
    public class FloatArrayCreation
    {
        public float[] ScaleArray(float[] array, float scalingFactor)
        {
            var result = new float[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i] * scalingFactor;
            return result;
        }

        private static readonly float[] Empty = new float[0];
    }
}
