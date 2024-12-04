class Program
{
    static List<string> ReadFileAndInitInput(string path)
    {
        List<string> result = new List<string>();

        try
        {
            result = new List<string>(File.ReadAllLines(path));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception caught while reading the file: {ex.Message}");
        }

        return result;
    }

    static int GetCountFromStarChecker(in List<string> list, int i, int j, int n, int m)
    {
        int count = 0;
        if (i >= 3)
        {
            if (list[i - 1][j] == 'M' && list[i - 2][j] == 'A' && list[i - 3][j] == 'S') ++count; // north
            if (j >= 3 && list[i - 1][j - 1] == 'M' && list[i - 2][j - 2] == 'A' && list[i - 3][j - 3] == 'S') ++count; // north - west
            if (j <= m - 4 && list[i - 1][j + 1] == 'M' && list[i - 2][j + 2] == 'A' && list[i - 3][j + 3] == 'S') ++count; // north - east
        }

        if (i <= n - 4)
        {
            if (list[i + 1][j] == 'M' && list[i + 2][j] == 'A' && list[i + 3][j] == 'S') ++count; // south
            if (j >= 3 && list[i + 1][j - 1] == 'M' && list[i + 2][j - 2] == 'A' && list[i + 3][j - 3] == 'S') ++count; // south - west
            if (j <= m - 4 && list[i + 1][j + 1] == 'M' && list[i + 2][j + 2] == 'A' && list[i + 3][j + 3] == 'S') ++count; // south - east
        }

        if (j >= 3 && list[i][j - 1] == 'M' && list[i][j - 2] == 'A' && list[i][j - 3] == 'S') ++count; // west
        if (j <= m - 4 && list[i][j + 1] == 'M' && list[i][j + 2] == 'A' && list[i][j + 3] == 'S') ++count; // east

        return count;
    }

    static int GetNumberOfXMAS(in List<string> list)
    {
        int res = 0, n = list.Count, m = list[0].Length;
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < m; ++j)
            {
                if (list[i][j] == 'X')
                {
                    int count = GetCountFromStarChecker(list, i, j, n, m);
                    res += count;
                }
            }
        }

        return res;
    }

    static bool IsXForm(in List<string> list, int i, int j)
    {
        if ( ( (list[i - 1][j - 1] == 'M' && list[i + 1][j + 1] == 'S') ||
               (list[i - 1][j - 1] == 'S' && list[i + 1][j + 1] == 'M') ) &&
             ( (list[i - 1][j + 1] == 'M' && list[i + 1][j - 1] == 'S') ||
               (list[i - 1][j + 1] == 'S' && list[i + 1][j - 1] == 'M') )
           ) return true;

        return false;
    }

    static int GetNumberOfX_MAS(in List<string> list)
    {
        int res = 0, n = list.Count, m = list[0].Length;
        for (int i = 1; i < n - 1; ++i)
        {
            for (int j = 1; j < m - 1; ++j)
            {
                if (list[i][j] == 'A')
                {
                    if (IsXForm(list, i, j)) ++res;
                }
            }
        }

        return res;
    }

    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day4\Input.txt";
        List<string> lines = ReadFileAndInitInput(filePath);

        int xmasCount = GetNumberOfXMAS(lines);
        Console.WriteLine($"The number of XMAS: {xmasCount}");

        int xmas_Count = GetNumberOfX_MAS(lines);
        Console.WriteLine($"The number of X_MAS: {xmas_Count}");
    }
}