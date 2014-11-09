using System;
using System.Linq;

namespace BDDoc.Generator
{
    class Program
    {
        private const string CBDDocFolderArgName = "/BDDocFolder:";

        static void Main(string[] args)
        {
            Console.WriteLine(args.FirstOrDefault());
            var bddocFolderArg = args.FirstOrDefault(s => s.ToUpper().StartsWith(CBDDocFolderArgName.ToUpper()));
            if (string.IsNullOrWhiteSpace(bddocFolderArg))
            {
                Console.WriteLine("Invalid arguments.");
                Environment.Exit(1);
            }
            var bddocFolder = bddocFolderArg.Remove(0, CBDDocFolderArgName.Length);
            if (string.IsNullOrWhiteSpace(bddocFolder))
            {
                Console.WriteLine("Invalid BDDocFolder argument.");
                Console.WriteLine("Error");
                Environment.Exit(1);
            }
            Environment.Exit(0);
        }
    }
}
