using System.Windows;
namespace MinesweeperWPF.Dialogs;

public partial class SettingsDialog : Window
{

    public SettingsDialog(int boardSize, int mineCount)
    {
        InitializeComponent();
        BoardSize = boardSize;
        MineCount = mineCount;

        BoardSizeIntegerUpDown.Value = BoardSize;
        MineCountIntegerUpDown.Value = MineCount;
    }
    /// <summary>
    ///     Размер игрового поля
    /// </summary>
    public int BoardSize { get; private set; }

    /// <summary>
    ///     Количество мин
    /// </summary>
    public int MineCount { get; private set; }

    private void BtnOk_Click(object sender, RoutedEventArgs e)
    {
        if (!BoardSizeIntegerUpDown.Value.HasValue || !MineCountIntegerUpDown.Value.HasValue)
        {
            MessageBox.Show("Необходимо указать корректные значения полей!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            return;
        }

        BoardSize = BoardSizeIntegerUpDown.Value.Value;
        MineCount = MineCountIntegerUpDown.Value.Value;
        DialogResult = true;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}