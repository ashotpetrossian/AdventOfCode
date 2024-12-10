using System.Collections.Generic;

class Program
{
    static List<(int, int)> directions = new List<(int, int)>
    {
        (1, 0), (-1, 0), (0, 1), (0, -1)
    };

    static List<List<char>> GetGridFromInput(in string filePath)
    {
        var result = new List<List<char>>();

        foreach (var line in File.ReadLines(filePath))
        {
            result.Add(new List<char>(line));
        }

        return result;
    }

    static void DFS(in List<List<char>> grid, ref List<List<int>> visited, int token, int i, int j, int n, int m, ref int count)
    {
        visited[i][j] = token;

        if (grid[i][j] == '9')
        {
            ++count;
            return;
        }

        foreach (var (ii, jj) in directions)
        {
            int ni = i + ii; int nj = j + jj;

            if (ni >= 0 && nj >= 0 && ni < n && nj < m && visited[ni][nj] != token && grid[ni][nj] == grid[i][j] + 1)
            {
                DFS(in grid, ref visited, token, ni, nj, n, m, ref count);
            }
        }
    }

    static void DFS2(in List<List<char>> grid, ref List<List<bool>> visited, int i, int j, int n, int m, ref int count)
    {
        if (grid[i][j] == '9')
        {
            ++count;
            return;
        }

        foreach (var (ii, jj) in directions)
        {
            int ni = i + ii; int nj = j + jj;

            if (ni >= 0 && nj >= 0 && ni < n && nj < m && visited[ni][nj] == false && grid[ni][nj] == grid[i][j] + 1)
            {
                visited[i][j] = true;
                DFS2(in grid, ref visited, ni, nj, n, m, ref count);
                visited[i][j] = false;
            }
        }
    }
    static void Main()
    {
        // Test part 1
        List<List<char>> gridTest = new List<List<char>>
        {
            new List<char> { '8', '9', '0', '1', '0', '1', '2', '3' },
            new List<char> { '7', '8', '1', '2', '1', '8', '7', '4' },
            new List<char> { '8', '7', '4', '3', '0', '9', '6', '5' },
            new List<char> { '9', '6', '5', '4', '9', '8', '7', '4' },
            new List<char> { '4', '5', '6', '7', '8', '9', '0', '3' },
            new List<char> { '3', '2', '0', '1', '9', '0', '1', '2' },
            new List<char> { '0', '1', '3', '2', '9', '8', '0', '1' },
            new List<char> { '1', '0', '4', '5', '6', '7', '3', '2' }
        };

        int nTest = gridTest.Count, mTest = gridTest[0].Count;

        List<List<int>> visitedTest = Enumerable.Range(0, nTest)
        .Select(_ => Enumerable.Repeat(0, mTest).ToList())
        .ToList();
        int tokenTest = 1;
        int resTest = 0;

        for (int i = 0; i < nTest; ++i)
        {
            for (int j = 0; j < mTest; ++j)
            {
                if (gridTest[i][j] == '0')
                {
                    int count = 0;
                    DFS(in gridTest, ref visitedTest, tokenTest, i, j, nTest, mTest, ref count);
                    resTest += count;
                    ++tokenTest;
                }
            }
        }

        Console.WriteLine($"Final test res: {resTest}");

        // Test part 2

        List<List<bool>> visitedTest2 = Enumerable.Range(0, nTest)
            .Select(_ => Enumerable.Repeat(false, mTest).ToList())
            .ToList();
        int resTest2 = 0;

        for (int i = 0; i < nTest; ++i)
        {
            for (int j = 0; j < mTest; ++j)
            {
                if (gridTest[i][j] == '0')
                {
                    int count = 0;
                    DFS2(in gridTest, ref visitedTest2, i, j, nTest, mTest, ref count);
                    resTest2 += count;
                }
            }
        }

        Console.WriteLine($"Expanded DFS2 resTest: {resTest2}");



        // Orig input, part 1

        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day10\Input.txt";
        List<List<char>> grid = GetGridFromInput(filePath);

        int n = grid.Count, m = grid[0].Count;

        List<List<int>> visited = Enumerable.Range(0, n)
        .Select(_ => Enumerable.Repeat(0, m).ToList())
        .ToList();

        int token = 1;
        int res = 0;

        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < n; ++j)
            {
                if (grid[i][j] == '0')
                {
                    int count = 0;
                    DFS(in grid, ref visited, token, i, j, n, m, ref count);

                    res += count;
                    ++token;
                }
            }
        }

        Console.WriteLine($"The final res: {res}");

        // Orig Input part 2

        List<List<bool>> visited2 = Enumerable.Range(0, n)
            .Select(_ => Enumerable.Repeat(false, m).ToList())
            .ToList();
        int res2 = 0;

        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < m; ++j)
            {
                if (grid[i][j] == '0')
                {
                    int count = 0;
                    DFS2(in grid, ref visited2, i, j, n, m, ref count);
                    res2 += count;
                }
            }
        }

        Console.WriteLine($"Expanded DFS2 res: {res2}");
    }
}
