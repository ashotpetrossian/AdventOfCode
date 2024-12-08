class Program
{
    struct Point
    {
        public int x;
        public int y;

        public Point(int a, int b) {  x = a; y = b; }
    }
    static Dictionary<char, List<Point>> AntennaToFrequency = new Dictionary<char, List<Point>>();
    static List<List<char>> list = new List<List<char>>();
    static int n = 0, m = 0;

    static void ReadFileAndParseTheData(in string filename)
    {
        foreach (var line in File.ReadAllLines(filename))
        {
            list.Add(new List<char>(line));
        }
    }

    static void FillTheFreq()
    {
        n = list.Count;
        m = list[0].Count;

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (char.IsDigit(list[i][j]) || char.IsLetter(list[i][j]))
                {
                    if (!AntennaToFrequency.ContainsKey(list[i][j]))
                    {
                        AntennaToFrequency[list[i][j]] = new List<Point>();
                    }
                    AntennaToFrequency[list[i][j]].Add(new Point(i, j));
                }
            }
        }
    }

    static int GetNumberOfAntiNodes()
    {
        int count = 0;
          
        foreach (var (c, points) in AntennaToFrequency)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = i + 1; j < points.Count; ++j)
                {
                    int xDiff = Math.Abs(points[i].x - points[j].x);
                    int yDiff = Math.Abs(points[i].y - points[j].y);

                    if ((points[i].x < points[j].x && points[i].y < points[j].y) || (points[i].x > points[j].x && points[i].y > points[j].y)) // left up or right down
                    {
                        int xTarget = Math.Min(points[i].x, points[j].x) - xDiff;
                        int yTarget = Math.Min(points[i].y, points[j].y) - yDiff;

                        if (xTarget >= 0 && yTarget >= 0 && list[xTarget][yTarget] != '#')
                        {
                            ++count;
                            list[xTarget][yTarget] = '#';
                        }

                        xTarget = Math.Max(points[i].x, points[j].x) + xDiff;
                        yTarget = Math.Max(points[i].y, points[j].y) + yDiff;

                        if (xTarget < n && yTarget < m && list[xTarget][yTarget] != '#')
                        {
                            ++count;
                            list[xTarget][yTarget] = '#';
                        }
                    }
                    else if ((points[i].x < points[j].x && points[i].y > points[j].y) || (points[i].x > points[j].x && points[i].y < points[j].y)) // right up or left down
                    {
                        int xTarget = Math.Min(points[i].x, points[j].x) - xDiff;
                        int yTarget = Math.Max(points[i].y, points[j].y) + yDiff;

                        if (xTarget >= 0 && yTarget < m && list[xTarget][yTarget] != '#')
                        {
                            ++count;
                            list[xTarget][yTarget] = '#';
                        }

                        xTarget = Math.Max(points[i].x, points[j].x) + xDiff;
                        yTarget = Math.Min(points[i].y, points[j].y) - yDiff;

                        if (xTarget < n && yTarget >= 0 && list[xTarget][yTarget] != '#')
                        {
                            ++count;
                            list[xTarget][yTarget] = '#';
                        }
                    }
                    else if (points[i].x == points[j].x) // same col
                    {
                        int y1 = Math.Min(points[i].y, points[j].y) - yDiff;
                        int y2 = Math.Max(points[i].y, points[j].y) + yDiff;

                        if (y1 >= 0 && list[points[i].x][y1] != '#')
                        {
                            ++count;
                            list[points[i].x][y1] = '#';
                        }

                        if (y2 < m && list[points[i].x][y2] != '#')
                        {
                            ++count;
                            list[points[i].x][y2] = '#';
                        }
                    }
                    else // same row
                    {
                        int x1 = Math.Min(points[i].x, points[j].x) - xDiff;
                        int x2 = Math.Max(points[i].x, points[j].x) + xDiff;

                        if (x1 >= 0 && list[x1][points[i].y] != '#')
                        {
                            ++count;
                            list[x1][points[i].y] = '#';
                        }

                        if (x2 < n && list[x2][points[i].y] != '#')
                        {
                            ++count;
                            list[x2][points[i].y] = '#';
                        }
                    }
                }
            }
        }

        return count;
    }

    static int GetNumberOfAntiNodesWithFreq()
    {
        foreach (var (c, points) in AntennaToFrequency)
        {
            for (int i = 0; i < points.Count; ++i)
            {
                for (int j = i + 1; j < points.Count; ++j)
                {
                    int xDiff = Math.Abs(points[i].x - points[j].x);
                    int yDiff = Math.Abs(points[i].y - points[j].y);

                    if ((points[i].x < points[j].x && points[i].y < points[j].y) || (points[i].x > points[j].x && points[i].y > points[j].y)) // left up or right down
                    {
                        int xTarget = Math.Min(points[i].x, points[j].x) - xDiff;
                        int yTarget = Math.Min(points[i].y, points[j].y) - yDiff;

                        while (xTarget >= 0 && yTarget >= 0)
                        {
                            if (list[xTarget][yTarget] != '#')
                            { 
                                list[xTarget][yTarget] = '#';
                            }

                            xTarget -= xDiff;
                            yTarget -= yDiff;
                        }

                        xTarget = Math.Max(points[i].x, points[j].x) + xDiff;
                        yTarget = Math.Max(points[i].y, points[j].y) + yDiff;

                        while (xTarget < n && yTarget < m)
                        {
                            if (list[xTarget][yTarget] != '#')
                            {
                                list[xTarget][yTarget] = '#';
                            }

                            xTarget += xDiff;
                            yTarget += yDiff;
                        }
                    }
                    else if ((points[i].x < points[j].x && points[i].y > points[j].y) || (points[i].x > points[j].x && points[i].y < points[j].y)) // right up or left down
                    {
                        int xTarget = Math.Min(points[i].x, points[j].x) - xDiff;
                        int yTarget = Math.Max(points[i].y, points[j].y) + yDiff;

                        while (xTarget >= 0 && yTarget < m)
                        {
                            if (list[xTarget][yTarget] != '#')
                            {
                                list[xTarget][yTarget] = '#';
                            }

                            xTarget -= xDiff;
                            yTarget += yDiff;
                        }

                        xTarget = Math.Max(points[i].x, points[j].x) + xDiff;
                        yTarget = Math.Min(points[i].y, points[j].y) - yDiff;

                        while (xTarget < n && yTarget >= 0)
                        {
                            if (list[xTarget][yTarget] != '#')
                            {
                                list[xTarget][yTarget] = '#';
                            }

                            xTarget += xDiff;
                            yTarget -= yDiff;
                        }
                    }
                    else if (points[i].x == points[j].x) // same col
                    {
                        int y1 = Math.Min(points[i].y, points[j].y) - yDiff;
                        int y2 = Math.Max(points[i].y, points[j].y) + yDiff;

                        while (y1 >= 0)
                        {
                            if (list[points[i].x][y1] != '#')
                            {
                                list[points[i].x][y1] = '#';
                            }

                            y1 -= yDiff;
                        }

                        while (y2 < m)
                        {
                            if (list[points[i].x][y2] != '#')
                            {
                                list[points[i].x][y2] = '#';
                            }

                            y2 += yDiff;
                        }
                    }
                    else // same row
                    {
                        int x1 = Math.Min(points[i].x, points[j].x) - xDiff;
                        int x2 = Math.Max(points[i].x, points[j].x) + xDiff;

                        while (x1 >= 0)
                        {
                            if (list[x1][points[i].y] != '#')
                            {
                                list[x1][points[i].y] = '#';
                            }

                            x1 -= xDiff;
                        }

                        while (x2 < n)
                        {
                            if (list[x2][points[i].y] != '#')
                            {
                                list[x2][points[i].y] = '#';
                            }

                            x2 += xDiff;
                        }
                    }
                }
            }
        }

        int res = 0;

        for (int i = 0; i < n; ++i)
        {
            for (int j = 0; j < m; ++j)
            {
                if (list[i][j] != '.') ++res;
            }
        }

        return res;
    }


    static void Main()
    {
        string filePath = @"C:\Users\Ashot\source\repos\AdventOfCode\Day8\Input.txt";
        ReadFileAndParseTheData(filePath);
        
        FillTheFreq();
        int count = GetNumberOfAntiNodes();
        Console.WriteLine($"Number of AntiDotes: {count}");
        int countWithFreq = GetNumberOfAntiNodesWithFreq();
        Console.WriteLine($"Number of AntiDotes with Freq: {countWithFreq}");

    }
}
