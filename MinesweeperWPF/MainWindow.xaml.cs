using MinesweeperWPF.Lib;
using System.Windows;

namespace MinesweeperWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int BoardSize = 10; // Размер игрового поля
    private const int MineCount = 15; // Количество мин

    private Game _game = new(BoardSize, MineCount);

    public MainWindow()
    {
        InitializeComponent();
        GenerateBoard();
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

    private void Cell_MouseLeftButtonDown(object sender, RoutedEventArgs e)
    {
        var cell = (Cell)sender;
        if (cell.HasMine)
        {
            // Игра проиграна
            _game.ShowAllMines();
            MessageBox.Show("Вы проиграли!", "Поражение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            ResetGame();
        }
        else
        {
            _game.OpenCell(cell);
            if (!_game.IsGameEnded) return;

            // Игра выиграна
            MessageBox.Show("Поздравляю, вы выиграли!", "Победа", MessageBoxButton.OK, MessageBoxImage.Information);
            ResetGame();
        }
    }

    private void ResetGame()
    {
        BoardGrid.Children.Clear();
        _game = new Game(BoardSize, MineCount);
        GenerateBoard();
    }
}
