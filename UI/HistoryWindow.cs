using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Data;

namespace Sokoban.UI;

public class HistoryWindow : Window
{
    private ProgInfo progInfo;

    public HistoryWindow(ProgInfo progInfo)
    {
        this.progInfo = progInfo;
        Title = T("history");
        Width = progInfo.Settings.WindowWidth;
        Height = progInfo.Settings.WindowHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Content = CreateContent();
    }

    private string T(string key) =>
        progInfo.Settings.Translations[progInfo.Settings.Language].TryGetValue(key, out var value) ? value : key;

        private Control CreateContent()
    {
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
            }
        };

        var layoutGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(new GridLength(1, GridUnitType.Star)) 
            },
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        var complLevelsCount = progInfo.User.LevelsStats.Count;
        var levelsCompletedText = new TextBlock
        {
            Text = $"{T("levelsCompleted")}: {complLevelsCount}",
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 20, 0, 20)
        };
        Grid.SetRow(levelsCompletedText, 0);
        layoutGrid.Children.Add(levelsCompletedText);

        // Таблица
        var dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All,
            BorderThickness = new Thickness(1),
            BorderBrush = Brushes.Gray,
            Margin = new Thickness(20)
        };

        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("levelName"),
            Binding = new Binding("LevelName"),
            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
        });
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("completionTime"),
            Binding = new Binding("Time"),
            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
        });
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("steps"),
            Binding = new Binding("Steps"),
            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
        });
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("completionDate"),
            Binding = new Binding("CompletionDate"),
            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
        });

        var historyItems = new List<HistoryItem>();
        foreach (var kvp in progInfo.User.LevelsStats)
        {
            foreach (var attempt in kvp.Value.Attempts)
            {
                historyItems.Add(new HistoryItem
                {
                    LevelName = kvp.Key,
                    Time = attempt.Time.ToString(@"mm\:ss"),
                    Steps = attempt.Steps.ToString(),
                    CompletionDate = attempt.CompletionDate.ToString("yyyy-MM-dd HH:mm")
                });
            }
        }

        historyItems = historyItems
            .OrderByDescending(i => DateTime.Parse(i.CompletionDate))
            .ToList();

        dataGrid.ItemsSource = historyItems;
        dataGrid.Margin = new Thickness(20, 20, 20, 90);

        Grid.SetRow(dataGrid, 1);
        layoutGrid.Children.Add(dataGrid);

        grid.Children.Add(layoutGrid);

        var backButton = new Button
        {
            Content = T("back"),
            Width = 150,
            Height = 50,
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

        grid.Children.Add(backButton);

        return grid;
    }

    private class HistoryItem
    {
        public string LevelName { get; set; } = "";
        public string Time { get; set; } = "";
        public string Steps { get; set; } = "";
        public string CompletionDate { get; set; } = "";
    }
}