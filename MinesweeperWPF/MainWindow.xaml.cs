using System.Windows;
using System.Windows.Input;

using MinesweeperWPF.Lib;

using Timer = System.Timers.Timer;
namespace MinesweeperWPF;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int BoardSize = 10; // Размер игрового поля
    private const int MineCount = 15; // Количество мин

    /// <summary>
    ///     Таймер с интервалом в 1 секунду
    /// </summary>
    private readonly Timer _timer = new(TimeSpan.FromSeconds(1));

    /// <summary>
    ///     Количество отмеченных мин
    /// </summary>
    private int _checkedMines;

    /// <summary>
    ///     Класс с реализацией игры
    /// </summary>
    private Game _game = new(BoardSize, MineCount);
    private DateTime _startTime;

    public MainWindow()
    {
        InitializeComponent();
        InitializeMainWindow();
        ResetGame();
    }

    private void InitializeMainWindow()
    {
        BoardGrid.Margin = new Thickness(5);
        _timer.Elapsed += Timer_Elapsed;
        _timer.AutoReset = true;
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
        Timer_Elapsed(null, null);

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
        if (!_timer.Enabled)
        {
            _startTime = DateTime.Now;
            _timer.Start();
        }

        var cell = (Cell)sender;
        var result = _game.OpenCell(cell);
        if (result)
        {
            // Игра проиграна
            _game.ShowAllMines();
            _timer.Stop();

            MessageBox.Show("Вы проиграли!", "Поражение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            ResetGame();
            return;
        }

        if (!_game.IsGameEnded) return;

        // Игра выиграна
        MessageBox.Show("Поздравляю, вы выиграли!", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
        ResetGame();
    }


    private void Timer_Elapsed(object? sender, EventArgs? e)
    {
        Dispatcher.Invoke(() =>
        {
            // Вычисление прошедшего времени и обновление TimerBlock
            var elapsedTime = DateTime.Now - _startTime;
            TimerBlock.Text = elapsedTime.ToString(@"hh\:mm\:ss");
        });
    }
}