using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeSnippets
{
    public class DspVectorCreation
    {
        public double[] ScaleArray(double[] array, double scalingFactor)
        {
            var result = new DspVector();
            result.SetData(array);
            result.InplaceScale(scalingFactor);
            return result.GetData();
        }

        /// <summary>
        /// This code isn't supposed to make lots
        /// of sense. We just create some statements to 
        /// make sure that they are ignored by our analysis.
        /// </summary>
        /// <param name="array">An array</param>
        public string ArrayToString(double[] array)
        {
            var buffer = new StringBuilder(); 
            foreach (var value in array)
            {
                var sep = " ";
                buffer.Append(sep.ToString() + value);
            }

            return buffer.ToString();
        }
    }

    public class DspVector
    {
        public DspVector()
        {
        }

        public DspVector(Type type)
        {
        }

        public void InplaceScale(double scale)
        {

        }

        public void SetData(double[] data)
        {

        }

        public double[] GetData()
        {
            return null;
        }
    }
}
