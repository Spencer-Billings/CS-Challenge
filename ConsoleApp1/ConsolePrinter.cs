using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    //This does not seem necessary/useful. Consider removing this class and just using Console.WriteLine.
    public class ConsolePrinter
    {
        public static object PrintValue;

        public ConsolePrinter Value(string value)
        {
            PrintValue = value;
            return this;
        }

        public override string ToString()
        {
            Console.WriteLine(PrintValue);
            return null;
        }
    }
}
