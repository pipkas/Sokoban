using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Sokoban.Controller;
using Sokoban.Models;

namespace Sokoban.UI;

public class StartWindow : Window
{
    private Settings settings;

    public StartWindow(Settings settings)
    {
        this.settings = settings;
        Title = "Sokoban";
        Width = settings.WindowWidth;
        Height = settings.WindowHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Content = CreateMainContent();
    }

    private string T(string key) =>
        settings.Translations[settings.Language].TryGetValue(key, out var value)
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

        var languageButton = new Button
        {
            Content = T("language"),
            Margin = new Thickness(0, 20, 20, 0),
            Padding = new Thickness(15, 8),
            FontSize = 12,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Top,
            Background = Brushes.LightGray,
            BorderBrush = Brushes.DarkGray
        };
        languageButton.Click += (s, e) =>
        {
            settings.Language = settings.Language == Language.Russian
                ? Language.Japanese
                : Language.Russian;
            Content = CreateMainContent();
        };
        grid.Children.Add(languageButton);

        var centerPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 40,
            Margin = new Thickness(0, -50, 0, 0)
        };

        var titleText = new TextBlock
        {
            Text = T("title"),
            FontSize = 72,
            FontWeight = FontWeight.Bold,
            Foreground = Brushes.DarkBlue,
            TextAlignment = TextAlignment.Center,
            FontFamily = new FontFamily("Arial")
        };
        centerPanel.Children.Add(titleText);

        var buttonPanel = new StackPanel
        {
            Spacing = 20,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var playButton = CreateStyledButton(T("play"), Brushes.LightGreen, Brushes.DarkGreen);
        playButton.Click += (s, e) => ShowNicknameInputWindow();

        var rulesButton = CreateStyledButton(T("rules"), Brushes.LightBlue, Brushes.DarkBlue);
        rulesButton.Click += (s, e) => ShowRulesWindow();

        var exitButton = CreateStyledButton(T("exit"), Brushes.LightCoral, Brushes.DarkRed);
        exitButton.Click += (s, e) => Close();

        buttonPanel.Children.Add(playButton);
        buttonPanel.Children.Add(rulesButton);
        buttonPanel.Children.Add(exitButton);
        centerPanel.Children.Add(buttonPanel);
        grid.Children.Add(centerPanel);

        return grid;
    }

    private Button CreateStyledButton(string text, ISolidColorBrush background, ISolidColorBrush foreground)
    {
        return new Button
        {
            Content = text,
            Width = 250,
            Height = 50,
            FontSize = 18,
            FontWeight = FontWeight.SemiBold,
            Background = background,
            Foreground = foreground,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(0)
        };
    }

    private void ShowNicknameInputWindow()
    {
        var nicknameWindow = new NicknameInputWindow(settings);
        nicknameWindow.NicknameConfirmed += (nickname) =>
        {
            var user = new User(nickname, LoadManager.LoadLevelStats(nickname, "Progress"));
            var progInfo = new ProgInfo(settings, user);
            var mainMenuWindow = new MainMenuWindow(progInfo); 
            this.Close();
        };
        nicknameWindow.Show();
        this.Hide();
    }

    private async void ShowRulesWindow()
    {
        var dialog = new Window
        {
            Width = 500,
            Height = 400,
            Title = T("rulesTitle"),
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