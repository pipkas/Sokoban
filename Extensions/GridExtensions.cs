using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Sokoban.Extensions;
public static class GridExtensions
{
    public static T WithGrid<T>(this T element, int row, int column) where T : Control
    {
        Grid.SetRow(element, row);
        Grid.SetColumn(element, column);
        return element;
    }
}