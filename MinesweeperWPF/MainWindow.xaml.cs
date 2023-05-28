using System.Windows;
using System.Windows.Input;

using MinesweeperWPF.Dialogs;
using MinesweeperWPF.Lib;

using Timer = System.Timers.Timer;
namespace MinesweeperWPF;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    /// <summary>
    ///     Таймер с интервалом в 1 секунду
    /// </summary>
    private readonly Timer _timer = new(TimeSpan.FromSeconds(1));

    /// <summary>
    ///     Класс с реализацией игры
    /// </summary>
    private Game? _game;
    private DateTime _startTime;

    public MainWindow()
    {
        InitializeComponent();
        InitializeMainWindow();
        ResetGame();
    }
    /// <summary>
    ///     Размер игрового поля
    /// </summary>
    private int BoardSize { get; set; } = 10;
    /// <summary>
    ///     Количество мин
    /// </summary>
    private int MineCount { get; set; } = 10;

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
        if (_game == null) return;

        _game.RemainingMinesChanged += UpdateCheckedMines;
        _game.DoActionForEachCells(cell =>
        {
            cell.PreviewMouseRightButtonDown += Cell_PreviewMouseRightButtonDown;
            cell.Click += Cell_MouseLeftButtonDown;
            BoardGrid.Children.Add(cell);
        });
    }

    private void ResetGame()
    {
        BoardGrid.Children.Clear();

        _game = new Game(BoardSize, MineCount);
        GenerateBoard();

        // Очищаем счётчик оставшихся мин
        UpdateCheckedMines(MineCount);

        // Очищаем таймер
        _timer.Stop();
        _startTime = DateTime.Now;
        Timer_Elapsed(null, null);
    }

    private void UpdateCheckedMines(int remainingMines) => RemainingMinesBlock.Text = $"Мины: {remainingMines}";

    private void Cell_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.RightButton != MouseButtonState.Pressed) return;

        var cell = (Cell)sender;
        _game?.SwitchCellFlag(cell);
    }

    private void Cell_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        if (_game == null) return;

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
        _timer.Stop();
        
        MessageBox.Show("Поздравляю, вы выиграли!", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
        ResetGame();
    }


    private void Timer_Elapsed(object? sender, EventArgs? e) => Dispatcher.Invoke(() =>
    {
        // Вычисление прошедшего времени и обновление TimerBlock
        var elapsedTime = DateTime.Now - _startTime;
        TimerBlock.Text = $"Время: {elapsedTime:hh\\:mm\\:ss}";
    });

    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SettingsDialog(BoardSize, MineCount)
        {
            Owner = this,
        };
        if (dialog.ShowDialog() != true) return;

        BoardSize = dialog.BoardSize;
        MineCount = dialog.MineCount;
        ResetGame();
    }

    private void BtnNew_Click(object sender, RoutedEventArgs e) => ResetGame();
}