using MinesweeperWPF.Lib;

using System.Windows;
using System.Windows.Threading;

namespace MinesweeperWPF;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int BoardSize = 10; // Размер игрового поля
    private const int MineCount = 15; // Количество мин

    /// <summary>
    /// Количество отмеченных мин
    /// </summary>
    private int _checkedMines;

    /// <summary>
    /// Таймер с интервалом в 1 секунду
    /// </summary>
    private readonly DispatcherTimer _timer = new()
    {
        Interval = TimeSpan.FromSeconds(1),
    };
    private DateTime _startTime;

    /// <summary>
    /// Класс с реализацией игры
    /// </summary>
    private Game _game = new(BoardSize, MineCount);

    public MainWindow()
    {
        InitializeComponent();
        InitializeMainWindow();
        ResetGame();
    }

    private void InitializeMainWindow()
    {
        _timer.Tick += Timer_Tick;
    }


    private void GenerateBoard()
    {
        BoardGrid.Rows = BoardSize;
        BoardGrid.Columns = BoardSize;

        foreach (var cell in _game.Board)
        {
            cell.Click += Cell_MouseLeftButtonDown;
            BoardGrid.Children.Add(cell);
        }
    }

    private void ResetGame()
    {
        BoardGrid.Children.Clear();

        // Очищаем количество отмеченных мин
        _checkedMines = 0;
        UpdateCheckedMines();

        // Очищаем таймер
        _startTime = DateTime.Now;
        Timer_Tick(null, null);

        _game = new Game(BoardSize, MineCount);
        GenerateBoard();
    }

    private void UpdateCheckedMines()
    {
        CheckedMinesBlock.Text = $"Количество мин: {_checkedMines}";
    }

    private void Cell_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        // Если таймер не запущен, то запускаем
        if (!_timer.IsEnabled)
        {
            _startTime = DateTime.Now;
            _timer.Start();
        }

        var cell = (Cell)sender;
        if (cell.HasMine)
        {
            // Игра проиграна
            _game.ShowAllMines();
            _timer.Stop();

            MessageBox.Show("Вы проиграли!", "Поражение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            ResetGame();
            return;
        }

        _game.OpenCell(cell);
        if (!_game.IsGameEnded) return;

        // Игра выиграна
        MessageBox.Show("Поздравляю, вы выиграли!", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
        ResetGame();
    }


    private void Timer_Tick(object? sender, EventArgs? e)
    {
        // Вычисление прошедшего времени и обновление TimerBlock
        TimeSpan elapsedTime = DateTime.Now - _startTime;
        TimerBlock.Text = elapsedTime.ToString(@"hh\:mm\:ss");
    }
}