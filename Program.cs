using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

namespace InvOFSC
{
    public class Program
    {
        private static Op op = new Op();
        static void Main(string[] args)
        {
            op.saveToCsv(op.readCSV());
        }
    }
}
