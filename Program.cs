using System.IO;
using System.Text.RegularExpressions;

class Program
{
    static void ReadFileAndInitString(in string filePath, ref string input)
    {
        try
        {
            input = File.ReadAllText(filePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while reading a file: " + ex.Message);
        }
    }

    static long FilterAndGetMulResult(in string input)
    {
        long result = 0;
        string pattern = @"mul\((\d+),(\d+)\)";

        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
           long firstNumber = long.Parse(match.Groups[1].Value);
           long secondNumber = long.Parse(match.Groups[2].Value);

           result += firstNumber * secondNumber;
        }

        return result;
    }

    static long FilterAndGetMulResultWithRestrictions(in string input)
    {
        long result = 0;
        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)";

        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(input);

        bool canMul = true;
        foreach (Match match in matches)
        {
            if (match.Value.StartsWith("do("))
            {
                canMul = true;
            }
            else if (match.Value.StartsWith("don"))
            {
                canMul = false;
            }            
            else 
            {
                if (canMul)
                {
                    long firstNumber = long.Parse(match.Groups[1].Value);
                    long secondNumber = long.Parse(match.Groups[2].Value);

                    result += firstNumber * secondNumber;
                }
            }
        }

        return result;
    }

    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day3\Input.txt";
        string input = string.Empty;
        ReadFileAndInitString(in filePath, ref input);

        long res = FilterAndGetMulResult(input);
        Console.WriteLine($"Product result: {res}");

        long res2 = FilterAndGetMulResultWithRestrictions(input);
        Console.WriteLine($"Product result with restrictions: {res2}");
    }
}