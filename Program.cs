using System;
using System.IO;

namespace PicMover
{
    class Program
    {
        static void Main(string[] args)
        {
       
            var mover = new Mover(@"D:\Photos");
            mover.Move(@"D:\import");
            Console.ReadKey();
        }

    }


}
