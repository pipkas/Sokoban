using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Sokoban.Models;
using Sokoban.Controller;

namespace Sokoban.UI;

public class NicknameInputWindow : Window
{
    #pragma warning disable CS0067  // Событие используется в другом классе, подавляем предупреждение
    public event Action<string>? NicknameConfirmed;
    #pragma warning restore CS0067
    private TextBox _nicknameTextBox = null!;
    private Button _nextButton = null!;
    private readonly Settings settings;

    public NicknameInputWindow(Settings settings)
    {
        this.settings = settings;
        Title = "Sokoban";
        Width = settings.WindowWidth;
        Height = settings.WindowHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        Content = CreateContent();
        Opened += (_, _) =>
        {
            _nicknameTextBox.Focus();
        };
    }

    private string T(string key) =>
        settings.Translations[settings.Language].TryGetValue(key, out var value)
            ? value
            : key;

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

        var centerPanel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Spacing = 30,
            Width = 400,
            Margin = new Thickness(0, 0, 0, 80)
        };

        var titleText = new TextBlock
        {
            Text = T("enterNickname"),
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            Foreground = Brushes.DarkSlateBlue,
            HorizontalAlignment = HorizontalAlignment.Center
        };
        centerPanel.Children.Add(titleText);

        _nicknameTextBox = new TextBox
        {
            FontSize = 18,
            Height = 45,
            MaxLength = 20,
            Watermark = T("nicknamePlaceholder"),
            TextAlignment = TextAlignment.Center,
            Background = Brushes.White,
            Foreground = Brushes.Black,
            CaretBrush = Brushes.Black,
            BorderBrush = Brushes.Gray,
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(8)
        };
        _nicknameTextBox.KeyDown += OnNicknameKeyDown;
        _nicknameTextBox.TextChanged += OnNicknameTextChanged;
        centerPanel.Children.Add(_nicknameTextBox);

        grid.Children.Add(centerPanel);

        _nextButton = new Button
        {
            Content = T("next"),
            FontSize = 16,
            Height = 45,
            Width = 150,
            Background = Brushes.LightGreen,
            Foreground = Brushes.DarkGreen,
            BorderBrush = Brushes.DarkGreen,
            BorderThickness = new Thickness(2),
            CornerRadius = new CornerRadius(8),
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(0, 0, 20, 20),
            IsVisible = false
        };
        _nextButton.Click += (s, e) => ConfirmNickname();
        grid.Children.Add(_nextButton);

        return grid;
    }

    private void OnNicknameKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && _nextButton.IsVisible)
        {
            ConfirmNickname();
        }
    }

    private void OnNicknameTextChanged(object? sender, TextChangedEventArgs e)
    {
        var hasText = !string.IsNullOrWhiteSpace(_nicknameTextBox.Text);
        _nextButton.IsVisible = hasText;
    }

    private void ConfirmNickname()
    {
        var nickname = _nicknameTextBox.Text?.Trim();
        if (string.IsNullOrWhiteSpace(nickname))
            return;

        var user = new User(nickname, LoadManager.LoadLevelStats(nickname, "Progress"));
        var progInfo = new ProgInfo(settings, user);
        var mainMenuWindow = new MainMenuWindow(progInfo);
        mainMenuWindow.Show();
        Close();
    }
}