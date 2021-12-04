using AdventOfCode.Utilities;

namespace AdventOfCode.Y2021.Day04;

class Solution : ISolver
{
    public int Year => 2021;

    public int Day => 4;

    public string GetName() => "Giant Squid";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    object PartOne(string input)
    {
        var lines = input.Lines(StringSplitOptions.RemoveEmptyEntries).ToArray();
        var numbersDrawn = lines[0].Integers().ToArray();
        var boards = ReadBoards(lines).ToArray();

        var drawingIndex = 0;
        while(true)
        {
            var draw = numbersDrawn[drawingIndex++];
            for(var boardIndex = 0; boardIndex < boards.Length; boardIndex++)
            {
                var board = boards[boardIndex];
                if (board.Draw(draw))
                {
                    return draw * board.UnmarkedSum();
                }
            }
        }
    }

    object PartTwo(string input)
    {
        var lines = input.Lines(StringSplitOptions.RemoveEmptyEntries).ToArray();
        var numbersDrawn = lines[0].Integers().ToArray();
        var boards = ReadBoards(lines).ToList();

        var drawingIndex = 0;
        int draw = -1;
        var removedBoard = boards[0];
        while (boards.Count > 0)
        {
            draw = numbersDrawn[drawingIndex++];
            for (var boardIndex = boards.Count-1; boardIndex >= 0; boardIndex--)
            {
                var board = boards[boardIndex];
                if (board.Draw(draw))
                {
                    boards.RemoveAt(boardIndex);
                    removedBoard = board;
                }
            }
        }

        return draw * removedBoard.UnmarkedSum();
    }

    private static IEnumerable<BingoBoard> ReadBoards(string[] lines)
    {
        int index = 1;
        while(index < lines.Length)
        {
            yield return new BingoBoard(index, lines);
            index += 5;
        }
    }

    private class BingoBoard
    {
        private readonly bool[,] drawn;
        private int[,] values;
        public BingoBoard(int index, string[] lines)
        {
            drawn = new bool[5, 5];
            values = new int[5, 5];
            for (int row = 0; row < 5; row++)
            {
                var rowValues = lines[index+row].Integers().ToArray();
                for (int col = 0; col < 5; col++)
                {
                    values[row, col] = rowValues[col];
                }
            }
        }

        public bool Draw(int value)
        {
            for(var row = 0; row < 5; row++)
            {
                for (var col = 0; col < 5; col++)
                {
                    if (values[row, col] == value)
                    {
                        drawn[row, col] = true;
                    }
                }
            }

            return HasWon();
        }

        public bool HasWon()
        {
            // Any row or column is fully marked
            var winner = Enumerable.Range(0, 5).Any(row =>
            {
                return Enumerable.Range(0, 5).All(col => drawn[row, col]);
            });

            winner = winner || Enumerable.Range(0, 5).Any(col =>
            {
                return Enumerable.Range(0, 5).All(row => drawn[row, col]);
            });

            return winner;
        }

        public int UnmarkedSum()
        {
            var unmarkedSum = Enumerable.Range(0, 5).Sum(row => Enumerable.Range(0, 5).Where(col => !drawn[row, col]).Sum(col => values[row, col]));
            return unmarkedSum;
        }
    }
}