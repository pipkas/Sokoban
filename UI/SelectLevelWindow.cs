using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Sokoban.Extensions;
using Sokoban.Models;

namespace Sokoban.UI;

public class SelectLevelWindow : Window
{
    private ProgInfo progInfo;
    public SelectLevelWindow(List<LevelData> levels, ProgInfo progInfo)
    {
        Title = "Sokoban";
        Width = progInfo.Settings.WindowWidth;
        Height = progInfo.Settings.WindowHeight;
        this.progInfo = progInfo;

        if (levels == null || levels.Count == 0)
        {
            Content = new TextBlock 
            { 
                Text = T("no levels found"), 
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16
            };
            return;
        }

        var grid = new Grid
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(0, 0, RelativeUnit.Relative),
                EndPoint = new RelativePoint(1, 1, RelativeUnit.Relative),
                GradientStops =
                {
                    new GradientStop(Colors.LightYellow, 0.0),
                    new GradientStop(Colors.LightGoldenrodYellow, 1.0)
                }
            },
            Margin = new Thickness(30),
            RowDefinitions = { new RowDefinition { Height = GridLength.Auto } }
        };

        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });

        var headerStyle = new Style(x => x.OfType<TextBlock>())
        {
            Setters =
            {
                new Setter(TextBlock.FontWeightProperty, FontWeight.Bold),
                new Setter(TextBlock.ForegroundProperty, Brushes.Black),
                new Setter(TextBlock.MarginProperty, new Thickness(5, 0, 5, 0))
            }
        };

        grid.Children.Add(new TextBlock { Text = "" }.AsHeader().WithGrid(0, 0));
        grid.Children.Add(new TextBlock { Text = "" }.AsHeader().WithGrid(0, 1));
        grid.Children.Add(new TextBlock { Text = "" }.AsHeader().WithGrid(0, 2));
        int rowIndex = 0;

        for (int i = 0; i < levels.Count; i++)
        {
            var level = levels[i];
            var levelData = level;
            if (!levelData.IsValid)
                continue;
            rowIndex++;

            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var levelIsCompleted = false;

            if (progInfo.User.LevelsStats.ContainsKey(level.Name))
                levelIsCompleted = true;
            
            var statusSymbol = levelIsCompleted ? "✓" : "";
            var statusText = new TextBlock
            {
                Text = statusSymbol,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Foreground = levelIsCompleted ? Brushes.Green : Brushes.Gray
            }.WithGrid(rowIndex, 0);

            var nameText = new TextBlock
            {
                Text = $"{i + 1}. {level.Name}",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 5, 10, 5)
            };

            var nameButton = new Button
            {
                Content = nameText,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = levelIsCompleted ? Brushes.LightGreen : Brushes.White,
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(8, 4)
            };

            nameButton.Click += (sender, e) =>
            {
                var session = new GameSession(levelData);
                var gameWindow = new GameWindow(session, progInfo);
                gameWindow.Show();
                Close();
            };

            grid.Children.Add(statusText);
            grid.Children.Add(nameButton.WithGrid(rowIndex, 1));

            string timeText = levelIsCompleted
                ? progInfo.User.LevelsStats[level.Name].BestTime.ToString(@"mm\:ss")
                : "—";

            var timeBlock = new TextBlock
            {
                Text = timeText,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 5, 10, 5),
                Foreground = levelIsCompleted ? Brushes.DarkBlue : Brushes.Gray
            }.WithGrid(rowIndex, 2);

            grid.Children.Add(timeBlock);
        }

        var scrollViewer = new ScrollViewer
        {
            Content = grid,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };

        var rootGrid = new Grid();

        rootGrid.Children.Add(scrollViewer);

        var backButton = new Button
        {
            Content = T("back"),
            Width = 140,
            Height = 45,
            Background = Brushes.LightBlue,
            Foreground = Brushes.DarkBlue,
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(20)
        };

        backButton.Click += (s, e) =>
        {
            new MainMenuWindow(progInfo).Show();
            Close();
        };

        rootGrid.Children.Add(backButton);

        Content = rootGrid;
    }

     private string T(string key) => 
        progInfo.Settings.Translations[progInfo.Settings.Language]
            .TryGetValue(key, out var value) ? value : key;
}



