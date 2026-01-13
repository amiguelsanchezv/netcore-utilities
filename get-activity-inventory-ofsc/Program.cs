using InvOFSC;
using Microsoft.Extensions.Configuration;

if (args.Length < 2)
{
    Console.WriteLine("Usage: InvOFSC <input_file> <output_file>");
    Console.WriteLine("  input_file:  Path to the input CSV file with activities");
    Console.WriteLine("  output_file: Path to the output CSV file (can use {0} as a placeholder for timestamp)");
    Environment.Exit(1);
}

var inputFile = args[0];
var outputFile = args[1];

if (!File.Exists(inputFile))
{
    Console.WriteLine($"Error: Input file '{inputFile}' does not exist.");
    Environment.Exit(1);
}

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var op = new Op(configuration, inputFile, outputFile);
op.saveToCsv(op.readCSV());
