using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace MinesweeperWPF.Lib;

public sealed class Cell : Button
{

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
    
    public int Row { get; }
    public int Column { get; }
    public bool HasMine { get; set; } = false;
    public bool HasFlag { get; set; } = false;
    public bool IsOpened
    {
        get => _isOpened;
        set
        {
            _isOpened = value;

            // Обновляем цвет и стиль ячейки
            Content = HasMine ? "*" : null;
            Foreground = HasMine ? Brushes.Red : Brushes.LightGray;
            FontWeight = FontWeights.Bold;

            // При попадании на мину игра заканчивается
            if (!HasMine) IsEnabled = false;
        }
    }
}