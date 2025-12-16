using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;


namespace Sokoban.Extensions;

public static class TextBlockExtensions
{
    public static TextBlock AsHeader(this TextBlock textBlock)
    {
        textBlock.FontWeight = FontWeight.Bold;
        textBlock.Foreground = Brushes.Black;
        textBlock.Margin = new Thickness(5, 0, 5, 0);
        textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
        textBlock.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
        return textBlock;
    }
}