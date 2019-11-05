using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCode
{
    class RandomStream
    {
        public string rnd(int length)
        {
            var random = new Random();
            string s = string.Empty;
            
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}
