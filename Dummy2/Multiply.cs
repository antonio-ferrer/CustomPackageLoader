using System;

namespace Dummy2
{
    public class Multiply
    {
        public Multiply(int val, uint times)
        {
            if (times == 0)
                Result = 0;
            else
            {
                int v = 0;
                for (var i = 0; i < times; i++)
                {
                    v = new Dummy1.Sum(v, val).Val;
                }
                Result = v;
            }

        }

        public int Result { get; }
    }
}
