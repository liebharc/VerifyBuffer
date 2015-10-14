using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeSnippets
{
    public class IntArrayCreation
    {
        public int[] ScaleArray(int[] array, int scalingFactor)
        {
            var result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i] * scalingFactor;
            return result;
        }
    }
}
