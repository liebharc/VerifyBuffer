using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCodeSnippets
{
    public class DataBufferUsage
    {
        private readonly IDataBuffer _buffer = DataBuffer.Create("OkayMethodCall");

        public void OkayMethodCall()
        {
            UseBufferMethod(_buffer.NewChild());
        }

        public void OkayMethodCall2()
        {
            var child = _buffer.NewChild();
            UseBufferMethod(child);
        }

        public void MissingChildConstruction()
        {
            UseBufferMethod(_buffer);
        }

        public void TooManyCallsInALine()
        {
            var data1 = _buffer.NewDoubles(100); var data2 = _buffer.NewDoubles(200);
        }

        public void OkayIteration()
        {
            for (int i = 0; i < 10; i++)
            {
                var child = _buffer.NewChildIteration(i);
                var data = child.NewDoubles(100);
            }
        }

        public void MissingIterationChild()
        {
            for (int i = 0; i < 10; i++)
            {
                var child = _buffer.NewChildIteration(i);
                var data = _buffer.NewDoubles(100);
            }
        }

        public void InvalidIterationChild()
        {
            for (int i = 0; i < 10; i++)
            {
                var child = _buffer.NewChild();
                var data = _buffer.NewDoubles(100);
            }
        }

        public void InvalidIterationAndMethodCall()
        {
            for (int i = 0; i < 10; i++)
            {
                UseBufferMethod(_buffer.NewChild());
            }
        }

        public void OkayMethodCallInIteration()
        {
            for (int i = 0; i < 10; i++)
            {
                var child = _buffer.NewChildIteration(i);
                UseBufferMethod(child.NewChild());
            }
        }

        public void UseBufferMethod(IDataBuffer buffer)
        {
            var data = buffer.NewDoubles(100);
            // do something with buffer
        }
    }

    public interface IDataBuffer
    {
        IDataBuffer NewChild([CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNo = 0);

        IDataBuffer NewChildIteration(int iteration);

        double[] NewDoubles(int size, [CallerLineNumber]int sourceLineNo = 0);
    }

    public class DataBuffer : IDataBuffer
    {
        private const string Seperator = "#";

        private class ChildBuffer : IDataBuffer
        {
            private readonly DataBuffer _parent;

            private readonly string _context;

            public ChildBuffer(DataBuffer parent, string context)
            {
                _parent = parent;

                _context = context;
            }

            public double[] NewDoubles(int size, [CallerLineNumber]int sourceLineNo = 0)
            {
                var fullKey = _context + Seperator + sourceLineNo;
                if (_parent._buffer.ContainsKey(fullKey))
                {
                    var cached = _parent._buffer[fullKey] as double[];
                    if (cached != null && cached.Length != size)
                        return cached;
                }

                var doubles = new double[size];
                _parent._buffer[fullKey] = doubles;
                return doubles;
            }

            public IDataBuffer NewChild([CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNo = 0)
            {
                var fullKey = _context + Seperator + sourceFilePath + Seperator + sourceLineNo;
                return new ChildBuffer(_parent, fullKey);
            }

            public IDataBuffer NewChildIteration(int iteration)
            {
                var fullKey = _context + Seperator + "It" + iteration;
                return new ChildBuffer(_parent, fullKey);
            }
        }

        private readonly ChildBuffer _firstChild;

        private readonly Dictionary<string, object> _buffer = new Dictionary<string, object>();

        private DataBuffer(string context)
        {
            _firstChild = new ChildBuffer(this, string.Empty);
        }

        public static IDataBuffer Create(string context)
        {
            return new DataBuffer(context);
        }

        public double[] NewDoubles(int size, [CallerLineNumber]int sourceLineNo = 0)
        {
            return _firstChild.NewDoubles(size, sourceLineNo);
        }

        public IDataBuffer NewChild([CallerFilePath]string sourceFilePath = "", [CallerLineNumber]int sourceLineNo = 0)
        {
            return _firstChild.NewChild(sourceFilePath, sourceLineNo);
        }

        public IDataBuffer NewChildIteration(int iteration)
        {
            return _firstChild.NewChildIteration(iteration);
        }
    }
}
