using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeSnippets
{
    public class StringArrayCreation
    {
        public string[] ScaleArray(string[] array, int subLength)
        {
            var result = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
                result[i] = array[i].Substring(0, Math.Min(array[i].Length, subLength));
            return result;
        }
    }
}
