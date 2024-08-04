using AC3;

var domains = GetDomains();
Dictionary<int, Func<int, bool>> unaryConstraints = new();
Dictionary<(int, int), Func<int, int, bool>> binaryConstraints = new();

/*var board = new int[,]
{
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },

    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },

    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
    {0, 0, 0,  0, 0, 0,  0, 0, 0 },
};*/

var board = new int[,]
{
    {0, 0, 0,  2, 0, 0,  0, 9, 0 },
    {9, 0, 3,  0, 6, 0,  2, 0, 7 },
    {0, 5, 4,  0, 0, 0,  8, 0, 0 },

    {4, 7, 0,  0, 0, 0,  0, 1, 0 },
    {0, 0, 2,  4, 0, 7,  0, 0, 0 },
    {5, 0, 0,  9, 0, 2,  0, 7, 0 },

    {0, 4, 0,  0, 0, 9,  7, 0, 0 },
    {0, 0, 1,  0, 0, 0,  5, 0, 0 },
    {0, 2, 6,  0, 5, 0,  0, 0, 0 },
};

for (int x = 0; x < 9; x++)
{
    for (int y = 0; y < 9; y++)
    {
        if (board[x, y] == 0)
            continue;

        var index = XYtoI(x, y);
        domains[index] = new List<int> { board[x, y] };
    }
}

for (int i = 0; i < 9; i++)
{
    // rows
    var row = GetRow(i);
    var rowConstraints = MutuallyExclusiveBinaryConstraints(row);
    foreach(var constraint in rowConstraints)
        binaryConstraints.TryAdd(constraint.Key, constraint.Value);

    // column
    var column = GetColumn(i);
    var columnConstraints = MutuallyExclusiveBinaryConstraints(column);
    foreach (var constraint in columnConstraints)
        binaryConstraints.TryAdd(constraint.Key, constraint.Value);

    // box
    var box = GetBox(i);
    var boxConstraints = MutuallyExclusiveBinaryConstraints(box);
    foreach (var constraint in boxConstraints)
        binaryConstraints.TryAdd(constraint.Key, constraint.Value);
}
var solver = new Solver<int>(
    domains,
    unaryConstraints,
    binaryConstraints);

solver.Solve();




for(int i = 0; i < domains.Length; i++)
{
    (int x, int y) v = ItoXY(i);

    if (domains[i].Count == 1)
    {
        board[v.x, v.y] = domains[i][0];
    }
}
for (int i = 0; i < 9; i++)
{
    for (int j = 0; j < 9; j++)
    {
        Console.Write(board[i, j] == 0 ? "♥ " : board[i, j] + " ");
        if (j % 3 == 2)
            Console.Write(" ");
    }
    if (i % 3 == 2)
        Console.WriteLine();
    Console.WriteLine();
}

;

List<int>[] GetDomains()
{
    var ret = new List<int>[81];
    for (int i = 0; i < 81; i++)
    {
        ret[i] = new List<int>();
        for (int j = 1; j <= 9; j++)
            ret[i].Add(j);
    }
    ret[0] = new List<int> { 1 };
    return ret;
}

List<int> GetBox(int i)
{
    var ret = new List<int>();
    (int x, int y) topleft = i switch
    {
        0 => (0, 0),
        1 => (3, 0),
        2 => (6, 0),
        3 => (0, 3),
        4 => (3, 3),
        5 => (6, 3),
        6 => (0, 6),
        7 => (3, 6),
        8 => (6, 6)
    };

    for (int dx = 0; dx < 3; dx++)
        for (int dy = 0; dy < 3; dy++)
            ret.Add(XYtoI(topleft.x + dx, topleft.y + dy));
    return ret;
}

List<int> GetColumn(int x)
{
    var ret = new List<int>();
    for (int y = 0; y < 9; y++)
        ret.Add(XYtoI(x, y));
    return ret;
}

List<int> GetRow(int y)
{
    var ret = new List<int>();
    for (int x = 0; x < 9; x++)
        ret.Add(XYtoI(x, y));
    return ret;
}

Dictionary<(int, int), Func<int, int, bool>> MutuallyExclusiveBinaryConstraints(List<int> numbers)
{
    var ret = new Dictionary<(int, int), Func<int, int, bool>>();
    foreach (var x in numbers)
        foreach(var y in numbers)
            if (x != y)
                ret.Add((x, y), (int xValue, int yValue) => xValue != yValue);
    return ret;
}

int XYtoI(int x, int y)
{
    return x + y * 9;
}

(int x, int y) ItoXY(int i)
{
    return (i % 9, i / 9);
}

