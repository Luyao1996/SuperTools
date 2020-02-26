using System;
using Luyao.SuperTools.IOHelper;

namespace Core.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Luyao! 123");

            try
            {
                var a = ConfigHelper.GetSection<string>("TestSection:Section2");
                var b = ConfigHelper.GetSection<string>("TestSection:Section1");
                var c = ConfigHelper.GetSection<string>("Test");
                var d = ConfigHelper.GetSection<string>("Test2");
                Console.WriteLine(a);
                Console.WriteLine(b);
                Console.WriteLine(c);
                Console.WriteLine(d);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            

        }
    }
}
