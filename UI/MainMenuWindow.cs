using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Sokoban.Controller;
using Sokoban.Models;

namespace Sokoban.UI;

public class MainMenuWindow : Window
{
    private ProgInfo progInfo;
    private List<LevelData> levels;

    public MainMenuWindow(ProgInfo progInfo)
    {
        this.progInfo = progInfo;
        this.levels = LoadManager.LoadLevelsData("Levels");
        Title = "Sokoban";
        Width = progInfo.Settings.WindowWidth;
        Height = progInfo.Settings.WindowHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Content = CreateMainContent();
    }

    private string T(string key) =>
        progInfo.Settings.Translations[progInfo.Settings.Language].TryGetValue(key, out var value)
            ? value
            : key;

    private Grid CreateMainContent()
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

        // Кнопка "Выйти" в верхнем левом углу
        var exitButton = CreateStyledButton(T("exit"), Brushes.LightCoral, Brushes.DarkRed);
        exitButton.HorizontalAlignment = HorizontalAlignment.Left;
        exitButton.VerticalAlignment = VerticalAlignment.Top;
        exitButton.Margin = new Thickness(20);
        exitButton.Click += (s, e) =>
        {
            var startWindow = new StartWindow(progInfo.Settings);
            startWindow.Show();
            Close();
        };
        grid.Children.Add(exitButton);

        // Кнопка "Настройки" в верхнем правом углу
        var settingsButton = CreateStyledButton(T("language"), Brushes.LightGray, Brushes.DarkGray);
        settingsButton.HorizontalAlignment = HorizontalAlignment.Right;
        settingsButton.VerticalAlignment = VerticalAlignment.Top;
        settingsButton.Margin = new Thickness(0, 20, 20, 0);
        settingsButton.Click += (s, e) =>
        {
            progInfo.Settings.Language = progInfo.Settings.Language == Language.Russian
                ? Language.Japanese
                : Language.Russian;
            Content = CreateMainContent();
        };
        grid.Children.Add(settingsButton);

        var rulesButton = CreateStyledButton(T("rules"), Brushes.LightBlue, Brushes.DarkBlue);
        rulesButton.HorizontalAlignment = HorizontalAlignment.Right;
        rulesButton.VerticalAlignment = VerticalAlignment.Top;
        rulesButton.Margin = new Thickness(0, 80, 20, 0);
        rulesButton.Click += (s, e) => ShowRulesWindow();
        grid.Children.Add(rulesButton);

        var selectLevelButton = CreateStyledButton(T("selectLevel"), Brushes.LightGreen, Brushes.DarkGreen);
        selectLevelButton.Width = 300;
        selectLevelButton.Height = 80;
        selectLevelButton.HorizontalAlignment = HorizontalAlignment.Center;
        selectLevelButton.VerticalAlignment = VerticalAlignment.Center;
        selectLevelButton.Click += (s, e) =>
        {
            var selectLevelWindow = new SelectLevelWindow(levels, progInfo);
            selectLevelWindow.Show();
            Close();
        };
        grid.Children.Add(selectLevelButton);

        var historyButton = CreateStyledButton(T("history"), Brushes.LightYellow, Brushes.DarkGoldenrod);
        historyButton.Width = 200;
        historyButton.Height = 60;
        historyButton.HorizontalAlignment = HorizontalAlignment.Left;
        historyButton.VerticalAlignment = VerticalAlignment.Bottom;
        historyButton.Margin = new Thickness(20, 0, 0, 20);
        historyButton.Click += (s, e) =>
        {
            var historyWindow = new HistoryWindow(progInfo);
            historyWindow.Show();
            Close();
        };
        grid.Children.Add(historyButton);

        // Кнопка "Рейтинг" внизу справа
        var ratingButton = CreateStyledButton(T("rating"), Brushes.LightYellow, Brushes.DarkGoldenrod);
        ratingButton.Width = 200;
        ratingButton.Height = 60;
        ratingButton.HorizontalAlignment = HorizontalAlignment.Right;
        ratingButton.VerticalAlignment = VerticalAlignment.Bottom;
        ratingButton.Margin = new Thickness(0, 0, 20, 20);
        ratingButton.Click += (s, e) =>
        {
            var ratingWindow = new RatingWindow(progInfo);
            ratingWindow.Show();
            Close();
        };
        grid.Children.Add(ratingButton);

        return grid;
    }

    private Button CreateStyledButton(string text, ISolidColorBrush background, ISolidColorBrush foreground)
    {
        return new Button
        {
            Content = text,
            FontSize = 18,
            FontWeight = FontWeight.SemiBold,
            Background = background,
            Foreground = foreground,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(15, 8)
        };
    }

    private async void ShowRulesWindow()
    {
        var dialog = new Window
        {
            Width = 500,
            Height = 400,
            Title = T("rules"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        var scrollViewer = new ScrollViewer
        {
            Content = new TextBlock
            {
                Text = T("rulesText"),
                FontSize = 14,
                Margin = new Thickness(20),
                TextWrapping = TextWrapping.Wrap
            }
        };
        dialog.Content = scrollViewer;
        await dialog.ShowDialog(this);
    }
}