using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

class Program
{
    static List<int>[] ReadFileAndSplitColumns(in string filePath)
    {
        List<int> list1 = new List<int>(); 
        List<int> list2 = new List<int>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line = string.Empty;
                while ((line = reader?.ReadLine()) != null)
                {
                    string[] numbers = line.Split(new[] { '\t', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    list1.Add(int.Parse(numbers[0]));
                    list2.Add(int.Parse(numbers[1]));
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

        return new List<int>[] { list1, list2 };
    }

    static int GetTotalDistance(in List<int>[] lists)
    {
        lists[0].Sort();
        lists[1].Sort();

        int res = 0, len = lists[0].Count;

        for (int i = 0; i < len; i++)
        {
            res += Math.Abs(lists[0][i] - lists[1][i]);
        }

        return res;
    }

    static int getSimilarityScore(in List<int>[] lists)
    {
        int res = 0, len = lists[0].Count;
        Dictionary<int, int> dict1 = new Dictionary<int, int>();
        Dictionary<int, int> dict2 = new Dictionary<int, int>();

        for (int i = 0; i < len; i++)
        {
            dict1[lists[0][i]] = dict1.GetValueOrDefault(lists[0][i], 0) + 1;
            dict2[lists[1][i]] = dict2.GetValueOrDefault(lists[1][i], 0) + 1;
        }

        foreach (var (num, count) in dict1)
        {
            if (dict2.ContainsKey(num))
            {
                res += count * dict2[num] * num;
            }
        }

        return res;
    }

    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day1\Numbers.txt";
        List<int>[] lists = ReadFileAndSplitColumns(filePath);

        int totalDist = GetTotalDistance(lists);
        int similarityScore = getSimilarityScore(lists);

        Console.WriteLine($"Total Distance: {totalDist}");
        Console.WriteLine($"Similarity Score: {similarityScore}");
    }
}
