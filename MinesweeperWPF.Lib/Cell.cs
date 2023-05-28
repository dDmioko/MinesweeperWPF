using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace MinesweeperWPF.Lib;

/// <summary>
///     Класс ячейки игры Сапёр
/// </summary>
public sealed class Cell : Button
{

    private readonly BitmapImage _bombBitmap = new(Helpers.GetResourceUri("cell-bomb.gif"));
    private readonly BitmapImage _mineBitmap = new(Helpers.GetResourceUri("cell-mine.gif"));

    private bool _hasFlag;
    private bool _isOpened;

    public Cell(int row, int column)
    {
        Row = row;
        Column = column;

        // Выравнивание содержимого по центру
        HorizontalContentAlignment = HorizontalAlignment.Center;
        VerticalContentAlignment = VerticalAlignment.Center;

        // Размер шрифта
        FontSize = 20;

        // Толщина границ кнопки
        BorderThickness = new Thickness(1);
        // Цвет границ кнопки
        BorderBrush = Brushes.Black;
        // Цвет кнопки
        Background = Brushes.LightBlue;
    }

    /// <summary>
    ///     Строка ячейка
    /// </summary>
    public int Row { get; }

    /// <summary>
    ///     Столбец ячейки
    /// </summary>
    public int Column { get; }

    /// <summary>
    ///     Является ли ячейка миной
    /// </summary>
    public bool HasMine { get; internal set; }

    /// <summary>
    ///     Установлен флаг на ячеку
    /// </summary>
    public bool HasFlag
    {
        get => _hasFlag;
        private set
        {
            _hasFlag = value;
            Content = value ? GetImage(_mineBitmap) : null;
        }
    }

    public bool IsOpened
    {
        get => _isOpened;
        internal set
        {
            _isOpened = value;

            // Обновляем цвет и стиль ячейки
            Content = HasMine ? GetImage(_bombBitmap) : null;
            Foreground = HasMine ? Brushes.Red : Brushes.LightGray;
            FontWeight = FontWeights.Bold;

            // При попадании на мину игра заканчивается
            if (!HasMine) IsEnabled = false;
        }
    }

    internal void SwitchFlag() => HasFlag = !HasFlag;

    private static Image GetImage(ImageSource bitmapImage) => new() { Source = bitmapImage };
}