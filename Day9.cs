class Program
{
    static int dot = -1, invalid = -2;
    static List<int> GetExpandString(in List<int> list)
    {
        List<int> vec = new List<int>();
        int id = 0;

        for (int k = 0; k < list.Count; ++k)
        {
            if (k % 2 == 0)
            {
                vec.AddRange(Enumerable.Repeat(id, list[k]));
                ++id;
            }
            else
            {
                vec.AddRange(Enumerable.Repeat(dot, list[k]));
            }
        }        

        return vec;
    }

    static void AddFragmentation(ref List<int> vec)
    {
        int i = 0, j = vec.Count - 1;

        while (i < j)
        {
            while (i < j && vec[i] != dot) ++i;
            while (i < j && vec[j] == dot) --j;

            (vec[i], vec[j]) = (vec[j], vec[i]);
        }
    }

    static int GetNumOfUpcomingDots(in List<int> list, int i)
    {
        int count = 0;
        while (i < list.Count && list[i] == dot)
        {
            ++count; ++i; 
        }
        return count;
    }

    static int GetNumOfPreceedingDigs(in List<int> list, int i, int c)
    {
        int count = 0;
        while (i >= 0 && list[i] == c)
        {
            ++count; --i;
        }
        return count;
    }

    static void RemoveFragmentation(ref List<int> vec)
    {
        int i = 0, j = vec.Count - 1;

        while (i < j)
        {
            while (i < j && (vec[i] != dot || vec[i] == invalid)) ++i;
            while (i < j && (vec[j] == dot || vec[j] == invalid)) --j;

            if (i == j) break;

            int numOfDots = GetNumOfUpcomingDots(in vec, i);
            int numOfDigs = GetNumOfPreceedingDigs(in vec, j, vec[j]);

            bool failedToFind = false;
            while (numOfDots < numOfDigs)
            {
                i += numOfDots;
                while (i < j && (vec[i] != dot || vec[i] == invalid)) ++i;
                if (i == j)
                {
                    failedToFind = true;
                    break;
                }

                numOfDots = GetNumOfUpcomingDots(in vec, i);
            }

            if (failedToFind) // failed to find a place for the currect file, moving to the next one.
            {
                j -= numOfDigs;
            }
            else // Pushing the currect file into the free segment
            {
                int diff = numOfDots - numOfDigs;
                while (numOfDigs-- > 0)
                {
                    vec[i++] = vec[j];
                    vec[j] = invalid;
                    --j;
                }
            }

            i = 0;
        }
    }

    static long GetCheckSum(in List<int> list)
    {
        long id = 0, res = 0;
        int i = 0;

        while (i < list.Count && list[i] != dot)
        {
            res += (id * (list[i]));
            ++id; ++i;
        }

        return res;
    }

    static long GetCheckSumOfDefragmentedFiles(in List<int> list)
    {
        long id = 0, res = 0;
        int i = 0;

        while (i < list.Count)
        {
            if (list[i] == dot || list[i] == invalid)
            {
                ++i; ++id;
                continue;
            }

            res += (id * (list[i]));
            ++id; ++i;
        }

        return res;
    }

    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day9\Input.txt";
        string str = File.ReadAllText(filePath);

        List<int> list = new List<int>();
        foreach (char c in str) { list.Add(c - '0'); }

        List<int> expandedList = GetExpandString(list);
        List<int> nonFragmentedList = new List<int>(expandedList);

        AddFragmentation(ref expandedList);
        long checkSum = GetCheckSum(expandedList);
        Console.WriteLine($"The final CheckSum = {checkSum}"); 

        RemoveFragmentation(ref nonFragmentedList);
        long defragCheckSum = GetCheckSumOfDefragmentedFiles(nonFragmentedList);
        Console.WriteLine($"Defragmented CheckSum: {defragCheckSum}");
    }
}
