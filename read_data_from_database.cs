using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantis
{
    class read_data_from_database
    {
        public int FindMax(int num1, int num2)
        {
            Console.WriteLine("Welcome to the C# Station Tutorial!");
            /* local variable declaration */
            int result;

            if (num1 > num2)
                result = num1;
            else
                result = num2;

            return result;
        }
    }
}
