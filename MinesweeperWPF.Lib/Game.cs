namespace MinesweeperWPF.Lib;

/// <summary>
/// Класс-релизация игры Сапёр
/// </summary>
public class Game
{
    /// <summary>
    /// Размер игрового поля
    /// </summary>
    public int BoardSize { get; }

    /// <summary>
    /// Количество мин
    /// </summary>
    public int MineCount { get; }

    private readonly Cell[,] board;
    /// <summary>
    /// Массив ячеек игрового поля
    /// </summary>
    public Cell[,] Board => board;

    private int remainingCells; // Количество оставшихся неоткрытых ячеек

    public Game(int boardSize, int mineCount)
    {
        BoardSize = boardSize;
        MineCount = mineCount;

        board = new Cell[BoardSize, BoardSize];
        remainingCells = BoardSize * BoardSize;

        GenerateBoard();
    }

    private void GenerateBoard()
    {
        // Создаем сетку ячеек
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                var cell = new Cell(row, col);
                board[row, col] = cell;
            }
        }

        // Размещаем мины случайным образом
        var random = new Random();
        int minesPlaced = 0;
        while (minesPlaced < MineCount)
        {
            int row = random.Next(BoardSize);
            int col = random.Next(BoardSize);
            var cell = board[row, col];
            if (!cell.HasMine)
            {
                cell.HasMine = true;
                minesPlaced++;
            }
        }
    }

    public void OpenCell(Cell cell)
    {
        if (cell.IsOpened || cell.HasFlag) return;

        cell.IsOpened = true;
        remainingCells--;

        // Подсчитываем количество мин вокруг ячейки
        int mineCount = 0;
        DoActionAroundCell(cell, (_, _) => mineCount++);

        // Обновляем текст ячейки
        if (mineCount > 0)
        {
            cell.Content = mineCount.ToString();
            return;
        }

        // Если вокруг ячейки нет мин, открываем соседние ячейки рекурсивно
        DoActionAroundCell(cell, (int i, int j) => OpenCell(board[i, j]));
    }

    public void ShowAllMines()
    {
        foreach (var cell in board.Cast<Cell>().Where(p => p.HasMine))
        {
            cell.IsOpened = true;
        }
    }

    private void DoActionAroundCell(Cell cell, Action<int, int> action)
    {
        int row = cell.Row;
        int col = cell.Column;

        for (int i = Math.Max(0, row - 1); i <= Math.Min(BoardSize - 1, row + 1); i++)
        {
            for (int j = Math.Max(0, col - 1); j <= Math.Min(BoardSize - 1, col + 1); j++)
            {
                action.Invoke(i, j);
            }
        }
    }
}
