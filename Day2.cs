using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<List<int>> ReadFileAndCollectData(in string filePath)
    {
        var lists = new List<List<int>>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = string.Empty;
                while ((line = reader?.ReadLine()) != null)
                {
                    var number = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => int.Parse(x))
                        .ToList();
                    lists.Add(number);
                }
            }
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Error while parsing a string to int: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Reading File: {ex.Message}");
        }

        return lists;
    }

    static int getNumberOfSafeReports(in List<List<int>> lists)
    {
        int count = 0;
        foreach (var item in lists)
        {
            bool increasing = true;
            bool success = true;
            int diff = 0;
            if (item.Count <= 1)
            {
                ++count;
                continue;
            }
            else
            {
                increasing = (item[1] > item[0]);
            }
            for (int i = 1; i < item.Count; ++i)
            {
                diff = item[i] - item[i - 1];
                if (!(increasing && diff >= 1 && diff <= 3) && !(!increasing && diff >= -3 && diff <= -1))
                {
                    success = false;
                    break;
                }
            }

            if (success) ++count;
        }

        return count;
    }

    static int getLongestIncreasingSubsequence(in List<int> list)
    {
        int n = list.Count, diff = 0;
        List<int> dp = Enumerable.Repeat(1, n).ToList();

        for (int i = 1; i < n; ++i)
        {
            for (int j = 0; j < i; ++j)
            {
                diff = list[i] - list[j];
                if (diff >= 1 && diff <= 3)
                {
                    dp[i] = Math.Max(dp[i], dp[j] + 1);
                }
            }
        }

        int res = 0;
        foreach (int num in dp) res = Math.Max(res, num);

        return res;
    }

    static int getLongestDecreasingSubsequence(in List<int> list)
    {
        int n = list.Count, diff = 0;
        List<int> dp = Enumerable.Repeat(1, n).ToList();

        for (int i = 1; i < n; ++i)
        {
            for (int j = 0; j < i; ++j)
            {
                diff = list[j] - list[i];
                if (diff >= 1 && diff <= 3)
                {
                    dp[i] = Math.Max(dp[i], dp[j] + 1);
                }
            }
        }

        int res = 0;
        foreach (int num in dp) res = Math.Max(res, num);

        return res;
    }

    static int getNumberOfAlmostSafeReports(in List<List<int>> lists)
    {
        int count = 0;

        foreach (var list in lists)
        {
            int numberOfLevels = list.Count;
            int lis = getLongestIncreasingSubsequence(list);
            int lds = getLongestDecreasingSubsequence(list);

            if (numberOfLevels - lis <= 1 || numberOfLevels - lds <= 1) ++count;
        }

        return count;
    }

    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day2\Input.txt";
        var lists = ReadFileAndCollectData(filePath);

        int safeReports = getNumberOfSafeReports(lists);
        Console.WriteLine($"Number of Safe reports: {safeReports}");

        int almostSafeReports = getNumberOfAlmostSafeReports(lists);
        Console.WriteLine($"Number of ALmost Safe reports: {almostSafeReports}");
    }
}
