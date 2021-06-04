using System;
using System.IO;

namespace IPSConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            Range r = new Range(1, 5);
            Console.WriteLine("Range end: " + r.End);
            if(args.Length < 1)
            {
                Console.WriteLine("Expected IPS filename");
                Environment.Exit(0);
            }
            Console.WriteLine("Processing " + args[0]);
            using (FileStream stream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                IPS.IPS patch = new IPS.IPS();
                patch.Parse(stream);
                foreach (IPS.Hunk hunk in patch.Hunks)
                {
                    Console.WriteLine("" + hunk);

                }
                patch.EditsAreDisjoint();
            }
        }
    }
}
