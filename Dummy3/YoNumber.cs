using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dummy3
{
    public class YoNumber
    {
        public int Sum { get; set; }

        public int Multiplied { get; set; }

        public YoNumber(int n1, int n2)
        {
            Sum = new Dummy1.Sum(n1, n2).Val;
            Multiplied = new Dummy2.Multiply(n1, Convert.ToUInt32(n2)).Result;
        }
    }
}
