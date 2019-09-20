using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApi_TestConcurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("測試 WebAPI:http://localhost:8088/CZAPI/liteonApi/SmartMeter/GetMeterKWH");
            Console.Write("輸入線程數:");
            int threadNum = 100;
            int.TryParse(Console.ReadLine(), out threadNum);
            while (Test(threadNum)) ;

            Console.ReadLine();
            Console.ReadLine();
        }

        private static bool Test(int TaskNumber)
        {
            return true;
        }
    }
}
