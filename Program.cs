using System.IO;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Transactions;
using static System.Collections.Specialized.BitVector32;

class Program
{
    static Dictionary<int, HashSet<int>> graph = new Dictionary<int, HashSet<int>>();

    static List<string> GetRulesFromInput(string filePath)
    {
        List<string> result = new List<string>();

        try
        {
            result = new List<string>(File.ReadAllLines(filePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught while reading the file: {ex.Message}");
        }

        return result;
    }

    static List<string> GetSectionsFromInput(string filePath)
    {
        List<string> result = new List<string>();

        try
        {
            result = new List<string>(File.ReadAllLines(filePath));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught while reading the file: {ex.Message}");
        }

        return result;
    }
    
    static void InitGraph(in List<string> list)
    {
        foreach (string s in list)
        {
            string[] numbers = s.Split('|');
            int u = int.Parse(numbers[0]);
            int v = int.Parse(numbers[1]);

            if (!graph.ContainsKey(u))
            {
                graph[u] = new HashSet<int>();
            }
            if (!graph.ContainsKey(v))
            {
                graph[v] = new HashSet<int>();
            }

            graph[u].Add(v);
        }
    }

    static int FindRightSectionsAndGetMidSum(List<string> sections)
    {
        int sum = 0;
        foreach (string s in sections)
        {
            string[] numbers = s.Split(',');
            List<int> nums = new List<int>();
            foreach (string number in numbers)
            {
                int num = int.Parse(number);
                nums.Add(num);

                if (!graph.ContainsKey(num))
                {
                    graph[num] = new HashSet<int>();
                }
            }

            bool success = true;
            for (int i = 1; success && i < nums.Count; ++i)
            {
                if (!graph[nums[i - 1]].Contains(nums[i]))
                {
                    success = false;
                }
            }

            if (success)
            {
                sum += nums[nums.Count / 2];
            }
        }

        return sum;
    }

    static int CorrectTheOrderAndFindTheMidElem(List<string> sections)
    {
        int sum = 0;
        foreach (string s in sections)
        {
            string[] numbers = s.Split(',');
            List<int> nums = new List<int>();
            foreach (string number in numbers)
            {
                int num = int.Parse(number);
                nums.Add(num);

                if (!graph.ContainsKey(num))
                {
                    graph[num] = new HashSet<int>();
                }
            }

            bool success = true;
            for (int i = 1; success && i < nums.Count; ++i)
            {
                if (!graph[nums[i - 1]].Contains(nums[i]))
                {
                    success = false;
                    break;
                }
            }

            if (!success)
            {
                for (int k = 0; k < nums.Count; ++k)
                {
                    for (int i = 1; i < nums.Count; ++i)
                    {
                        if (!graph[nums[i - 1]].Contains(nums[i]))
                        {
                            (nums[i], nums[i - 1]) = (nums[i - 1], nums[i]);
                        }
                    }
                }

                sum += nums[nums.Count / 2];
            }
        }

        return sum;
    }

    static void Main()
    {

        string filePath1 = @"C:\Users\Ashot\source\repos\AdventOfCode\Day5\RulesInput.txt";
        string filePath2 = @"C:\Users\Ashot\source\repos\AdventOfCode\Day5\SectionsInput.txt";
        List<string> rules = GetRulesFromInput(filePath1);
        List<string> sections = GetSectionsFromInput(filePath2);
        InitGraph(rules);

        int sum = FindRightSectionsAndGetMidSum(sections);
        Console.WriteLine($"The sum of Mids: {sum}");

        int correctedSum = CorrectTheOrderAndFindTheMidElem(sections);
        Console.WriteLine($"The sum of mids after corrections: {correctedSum}");
    }
}