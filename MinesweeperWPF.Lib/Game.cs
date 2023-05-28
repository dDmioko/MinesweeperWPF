namespace MinesweeperWPF.Lib;

/// <summary>
///     Класс-релизация игры Сапёр
/// </summary>
public class Game
{

    // Количество ячеек с флагом
    private int _flaggedCells;
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
    ///     Количество оставшихся мин
    /// </summary>
    public int RemainingMines => MineCount - _flaggedCells;

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
    private Cell[,] Board { get; }

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

    /// <summary>
    ///     Открывает указанную ячейку
    /// </summary>
    /// <param name="cell">Ячейка</param>
    /// <returns>Попались на мину</returns>
    public bool OpenCell(Cell cell)
    {
        var result = cell.HasMine;
        if (cell.HasMine || cell.IsOpened || cell.HasFlag) return result;

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
            return result;
        }

        // Если вокруг ячейки нет мин, открываем соседние ячейки рекурсивно
        DoActionAroundCell(cell, (i, j) => OpenCell(Board[i, j]));
        return result;
    }

    /// <summary>
    ///     Показать все мины
    /// </summary>
    public void ShowAllMines()
    {
        foreach (var cell in Board.Cast<Cell>().Where(p => p.HasMine))
        {
            cell.IsOpened = true;
        }
    }

    /// <summary>
    ///     Устанавливает флаг на ячейку
    /// </summary>
    /// <param name="cell">Ячейка</param>
    public void SwitchCellFlag(Cell cell)
    {
        cell.SwitchFlag();
        _flaggedCells += cell.HasFlag ? 1 : -1;
    }

    public void DoActionForEachCells(Action<Cell> action)
    {
        foreach (var cell in Board)
        {
            action.Invoke(cell);
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