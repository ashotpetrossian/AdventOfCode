using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

class Program
{
    static List<long> sums = new List<long>();
    static List<List<long>> lists = new List<List<long>>();
    static List<string> operations = new List<string> { "+", "*", "||" };

    static void ReadFileAndParseTheData(in string filename)
    {
        foreach (var line in File.ReadAllLines(filename))
        {
            // Split the line on the colon
            var parts = line.Split(':');
            if (parts.Length != 2) continue;

            // Parse the key (before the colon)
            long key = long.Parse(parts[0].Trim());

            // Parse the values (after the colon)
            var values = new List<long>();
            foreach (var value in parts[1].Trim().Split(' '))
            {
                values.Add(long.Parse(value));
            }

            sums.Add(key);
            lists.Add(values);
        }
    }

    static bool CanPerformTheRes(in List<long> list, long sum, long target, int i, int n)
    {
        if (sum > target) return false;
        if (sum == target && i == n) return true;
        else if (i >= n) return false;


        bool add = CanPerformTheRes(list, sum + list[i], target, i + 1, n);
        bool mul = CanPerformTheRes(list, sum * list[i], target, i + 1, n);

        return add || mul;
    }

    static void Dfs(ref List<List<string>> equations, List<string> values, in List<long> list, int i, int n)
    {
        values.Add(list[i].ToString());

        if (i == n - 1)
        {
            equations.Add(new List<string>(values));
            values.RemoveAt(values.Count - 1);
            return;
        }

        foreach (string operation in operations)
        {
            values.Add(operation.ToString());
            Dfs(ref equations, values, list, i + 1, n);
            values.RemoveAt(values.Count - 1);
        }

        values.RemoveAt(values.Count - 1);
    }

    static long GetSumOfEquation(in List<string> list)
    {
        long sum = long.Parse(list[0]);

        for (int i = 1; i < list.Count - 1; i += 2)
        {
            if (list[i] == "+")
            {
                sum += long.Parse(list[i + 1]);
            }
            else if (list[i] == "*")
            {
                sum *= long.Parse(list[i + 1]);
            }
            else
            {
                string newVal = sum.ToString() + list[i + 1];
                sum = long.Parse(newVal);
            }
        }

        return sum;
    }

    static bool CanPerformTheResWithThirdOperator(in List<long> list, long target)
    {
        List<List<string>> equations = new List<List<string>>();
        List<string> values = new List<string>();

        Dfs(ref equations, values, list, 0, list.Count);

        foreach (List<string> eq in equations)
        {
            long sum = GetSumOfEquation(in eq);
            if (sum == target) return true;
        }

        return false;
    }

    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day7\Input.txt";
        ReadFileAndParseTheData(filePath);
        
        long res = 0;
        for (int i = 0; i < sums.Count; i++)
        {
            if (CanPerformTheRes(lists[i], lists[i][0], sums[i], 1, lists[i].Count))
            {
                res += sums[i];
            }
        }

        Console.WriteLine($"The res sum: {res}");

        long res2 = 0;
        for (int i = 0; i < sums.Count; i++)
        {
            if (CanPerformTheResWithThirdOperator(lists[i], sums[i]))
            {
                res2 += sums[i];
            }
        }

        Console.WriteLine($"The res sum with third operator: {res2}");
    }
}
