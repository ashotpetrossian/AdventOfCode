using System.Formats.Asn1;
using System.Net.Http.Headers;
using System.Threading.Tasks.Dataflow;

class Program
{
    static int[] rowDirs = { 0, 0, 1, -1 };
    static int[] colDirs = { 1, -1, 0, 0 };

    struct Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"X: {x}, Y: {y}";
    }

    static List<List<char>> GetInputFromFile(in string filePath)
    {
        var input = new List<List<char>>();
        try
        {
            foreach (var line in File.ReadLines(filePath))
            {
                input.Add(new List<char>(line));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading the file: {ex.Message}");
        }

        return input;
    }

    static (int, int) GetAreaAndPerimiter(in List<List<char>> grid, ref List<List<bool>> visited, char letter, int i, int j, int n, int m)
    {
        int area = 0, perimiter = 0;
        DFS(grid, ref visited, letter, i, j, n, m, ref area, ref perimiter);
        return (area, perimiter);
    }

    static void DFS(in List<List<char>> grid, ref List<List<bool>> visited, char letter, int i, int j, int n, int m, ref int area, ref int perimiter)
    {
        visited[i][j] = true;

        for (int k = 0; k < 4; ++k)
        {
            int ni = i + rowDirs[k];
            int nj = j + colDirs[k];

            if (ni < 0 || nj < 0 || ni >= n || nj >= m)
            {
                ++perimiter;
                continue;
            }
            else if (grid[ni][nj] != letter)
            {
                ++perimiter;
            }
            else if (!visited[ni][nj])
            {
                ++area;
                DFS(grid, ref visited, letter, ni, nj, n, m, ref area, ref perimiter);
            }
        }
    }
    static (int, int) GetAreaAndSides(in List<List<char>> grid, ref List<List<bool>> visited, char letter, int i, int j, int n, int m)
    {
        int area = 0, sides = 0;
        Dictionary<Point, int> mapper = new Dictionary<Point, int>();

        DFS2(grid, ref visited, letter, i, j, n, m, ref area, ref mapper);

        foreach (var (point, count) in mapper)
        {
            sides += count;
        }


        return (area, sides);
    }

    static void CollectPointsIfCorner(in List<List<char>> grid, char letter, int i, int j, int n, int m, ref Dictionary<Point, int> mapper)
    {
        if ( (i == 0 || grid[i - 1][j] != letter) && (j == 0 || grid[i][j - 1] != letter) ) // left-top corner
        {
            Point p = new Point(i - 1, j - 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if ( (i == n - 1 || grid[i + 1][j] != letter) && (j == 0 || grid[i][j - 1] != letter) ) // left-bottom corner
        {
            Point p = new Point(i + 1, j - 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if ( (i == 0 || grid[i - 1][j] != letter) && (j == m - 1 || grid[i][j + 1] != letter) ) // right-top corner
        {
            Point p = new Point(i - 1, j + 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if ( (i == n - 1 || grid[i + 1][j] != letter) && (j == m - 1 || grid[i][j + 1] != letter) ) // right-bottom corner
        {
            Point p = new Point(i + 1, j + 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if (i > 0 && j > 0 && grid[i - 1][j] == letter && grid[i][j - 1] == letter && grid[i - 1][j - 1] != letter)
        {
            Point p = new Point(i - 1, j - 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if (i > 0 && j < m - 1 && grid[i - 1][j] == letter && grid[i][j + 1] == letter && grid[i - 1][j + 1] != letter)
        {
            Point p = new Point(i - 1, j + 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if (i < n - 1 && j > 0 && grid[i + 1][j] == letter && grid[i][j - 1] == letter && grid[i + 1][j - 1] != letter)
        {
            Point p = new Point(i + 1, j - 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }

        if (i < n - 1 && j < m - 1 && grid[i + 1][j] == letter && grid[i][j + 1] == letter && grid[i + 1][j + 1] != letter)
        {
            Point p = new Point(i + 1, j + 1);
            if (mapper.ContainsKey(p))
            {
                ++mapper[p];
            }
            else
            {
                mapper.Add(p, 1);
            }
        }
    }

    static void DFS2(in List<List<char>> grid, ref List<List<bool>> visited, char letter, int i, int j, int n, int m, ref int area, ref Dictionary<Point, int> mapper)
    {
        visited[i][j] = true;
        CollectPointsIfCorner(grid, letter, i, j, n, m, ref mapper);

        for (int k = 0; k < 4; ++k)
        {
            int ni = i + rowDirs[k];
            int nj = j + colDirs[k];

            if (ni >= 0 && nj >= 0 && ni < n && nj < m && grid[ni][nj] == letter && !visited[ni][nj])
            {
                ++area;
                DFS2(grid, ref visited, letter, ni, nj, n, m, ref area, ref mapper);
            }
        }
    }
    

    static void Main()
    {
        // Test Part 1
        var inputTest = new List<List<char>>
        {
            new List<char> { 'R', 'R', 'R', 'R', 'I', 'I', 'C', 'C', 'F', 'F' },
            new List<char> { 'R', 'R', 'R', 'R', 'I', 'I', 'C', 'C', 'C', 'F' },
            new List<char> { 'V', 'V', 'R', 'R', 'R', 'C', 'C', 'F', 'F', 'F' },
            new List<char> { 'V', 'V', 'R', 'C', 'C', 'C', 'J', 'F', 'F', 'F' },
            new List<char> { 'V', 'V', 'V', 'V', 'C', 'J', 'J', 'C', 'F', 'E' },
            new List<char> { 'V', 'V', 'I', 'V', 'C', 'C', 'J', 'J', 'E', 'E' },
            new List<char> { 'V', 'V', 'I', 'I', 'I', 'C', 'J', 'J', 'E', 'E' },
            new List<char> { 'M', 'I', 'I', 'I', 'I', 'I', 'J', 'J', 'E', 'E' },
            new List<char> { 'M', 'I', 'I', 'I', 'S', 'I', 'J', 'E', 'E', 'E' },
            new List<char> { 'M', 'M', 'M', 'I', 'S', 'S', 'J', 'E', 'E', 'E' }
        };

        int nTest = inputTest.Count, mTest = inputTest[0].Count;
        List<List<bool>> visitedTest = Enumerable
            .Range(0, nTest)
            .Select(_ => Enumerable.Repeat(false, mTest).ToList())
            .ToList();

        int resTest = 0;
        for (int i = 0; i < nTest; ++i)
        {
            for (int j = 0; j < mTest; ++j)
            {
                if (!visitedTest[i][j])
                {
                    char c = inputTest[i][j];
                    var (area, perimeter) = GetAreaAndPerimiter(inputTest, ref visitedTest, c, i, j, nTest, mTest);

                    resTest += (area + 1) * perimeter;
                }
            }
        }

        Console.WriteLine($"resTest: {resTest}");

        // Test part2

        visitedTest = visitedTest.Select(row => row.Select(_ => false).ToList()).ToList();
        int resTest2 = 0;
        for (int i = 0; i < nTest; ++i)
        {
            for (int j = 0; j < mTest; ++j)
            {
                if (!visitedTest[i][j])
                {
                    char c = inputTest[i][j];
                    var (area, sides) = GetAreaAndSides(inputTest, ref visitedTest, c, i, j, nTest, mTest);

                    resTest2 += (area + 1) * sides;
                }
            }
        }

        Console.WriteLine($"resTest2: {resTest2}");

        // Original Part 1

        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day12\Input.txt";
        var grid = GetInputFromFile(filePath);
        int n = grid.Count, m = grid[0].Count;
        List<List<bool>> visited = Enumerable
            .Range(0, n)
            .Select(_ => Enumerable.Repeat(false, m).ToList())
            .ToList();

        int res = 0;
        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < m; ++j)
            {
                if (!visited[i][j])
                {
                    char c = grid[i][j];
                    var (area, perimeter) = GetAreaAndPerimiter(grid, ref visited, c, i, j, n, m);

                    res += (area + 1) * perimeter;
                }
            }
        }

        Console.WriteLine($"Res part 1: {res}");

        visited = visited.Select(row => row.Select(_ => false).ToList()).ToList();
        int res2 = 0;

        // Orig Part 2

        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < m; ++j)
            {
                if (!visited[i][j])
                {
                    char c = grid[i][j];
                    var (area, sides) = GetAreaAndSides(grid, ref visited, c, i, j, n, m);

                    res2 += (area + 1) * sides;
                }
            }
        }

        Console.WriteLine($"Res part 2: {res2}");
    }
}