namespace MinesweeperWPF.Lib;

/// <summary>
///     Класс-релизация игры Сапёр
/// </summary>
public class Game
{
    // Количество оставшихся неоткрытых ячеек
    private int _remainingCells;

    public Game(int boardSize, int mineCount)
    {
        BoardSize = boardSize;
        MineCount = mineCount;

        Board = new Cell[BoardSize, BoardSize];
        _remainingCells = Board.Length;

        GenerateBoard();
    }
    
    /// <summary>
    ///     Размер игрового поля
    /// </summary>
    private int BoardSize { get; }

    /// <summary>
    ///     Количество мин
    /// </summary>
    private int MineCount { get; }

    /// <summary>
    ///     Массив ячеек игрового поля
    /// </summary>
    public Cell[,] Board { get; }
    /// <summary>
    ///     Открыты ли все ячейки, кроме мин
    /// </summary>
    public bool IsGameEnded => _remainingCells == MineCount;

    private void GenerateBoard()
    {
        // Создаём ячейки
        for (var row = 0; row < BoardSize; row++)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                var cell = new Cell(row, col);
                Board[row, col] = cell;
            }
        }

        // Размещаем мины случайным образом
        var minesPlaced = 0;
        while (minesPlaced < MineCount)
        {
            var cell = Board[Random.Shared.Next(BoardSize), Random.Shared.Next(BoardSize)];
            if (cell.HasMine) continue;

            // Если вокруг ячейки нет пустых ячеек, то ищем другое место
            var emptyCount = 0;
            DoActionAroundCell(cell, (i, j) =>
            {
                if (Board[i, j].HasMine) return;
                emptyCount++;
            });
            if (emptyCount == 0) continue;

            cell.HasMine = true;
            minesPlaced++;
        }
    }

    public void OpenCell(Cell cell)
    {
        if (cell.IsOpened || cell.HasFlag) return;

        cell.IsOpened = true;
        _remainingCells--;

        // Подсчитываем количество мин вокруг ячейки
        var mineCount = 0;
        DoActionAroundCell(cell, (i, j) =>
        {
            if (!Board[i, j].HasMine) return;
            mineCount++;
        });

        // Обновляем текст ячейки
        if (mineCount > 0)
        {
            cell.Content = mineCount.ToString();
            return;
        }

        // Если вокруг ячейки нет мин, открываем соседние ячейки рекурсивно
        DoActionAroundCell(cell, (i, j) => OpenCell(Board[i, j]));
    }

    public void ShowAllMines()
    {
        foreach (var cell in Board.Cast<Cell>().Where(p => p.HasMine))
        {
            cell.IsOpened = true;
        }
    }

    private void DoActionAroundCell(Cell cell, Action<int, int> action)
    {
        var row = cell.Row;
        var col = cell.Column;

        for (var i = Math.Max(0, row - 1); i <= Math.Min(BoardSize - 1, row + 1); i++)
        {
            for (var j = Math.Max(0, col - 1); j <= Math.Min(BoardSize - 1, col + 1); j++)
            {
                action.Invoke(i, j);
            }
        }
    }
}