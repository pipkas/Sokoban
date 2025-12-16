using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Sokoban.Controller;
using Sokoban.Models;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace Sokoban.UI;

public class GameWindow : Window
{
    private GameSession session;
    private ProgInfo progInfo;
    private Frame frame;
    private HashSet<Key> pressedKeys = [];
    private Key? lastPressedKey;
    private List<LevelData> levels = LoadManager.LoadLevelsData("Levels");

    public GameWindow(GameSession session, ProgInfo progInfo)
    {
        this.session = session ?? throw new ArgumentNullException(nameof(session));
        if (session.Level == null)
            throw new InvalidOperationException("GameSession has no level.");
        this.progInfo = progInfo;
        Width = progInfo.Settings.WindowWidth;
        Height = progInfo.Settings.WindowHeight;
        Title = "Sokoban";

        var mainGrid = new Grid
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
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
            }
        };

        var controlPanel = CreateControlPanel();
        mainGrid.Children.Add(controlPanel);
        Grid.SetRow(controlPanel, 0);

        frame = new Frame(session, progInfo);
        mainGrid.Children.Add(frame);
        Grid.SetRow(frame, 1);

        Content = mainGrid;

        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;

        session.Start();
    }

    private StackPanel CreateControlPanel()
    {
        var panel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(10),
            Spacing = 10
        };

        var backButton = new Button
        {
            Content = T("back to levels"),
            Padding = new Thickness(10, 5),
            FontSize = 14,
            Background = Brushes.LightGray,
            Foreground = Brushes.Black
        };
        backButton.Click += (sender, e) =>
        {
            SaveManager.SaveLevelStats(progInfo.User, "Progress");
            ReturnToLevelSelection();
        };

        var levelInfo = new TextBlock
        {
            Text = T("level") + $": {session.Level.Name}",
            FontSize = 14,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10, 0)
        };

        var movesInfo = new TextBlock
        {
            Text = T("moves") + $": {session.MoveCount}",
            FontSize = 14,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10, 0)
        };

        var resetButton = new Button
        {
            Content = T("resetLevel"),
            Padding = new Thickness(10, 5),
            FontSize = 14,
            Background = Brushes.LightCoral,
            Foreground = Brushes.Black,
            Margin = new Thickness(20, 0, 0, 0)
        };
        resetButton.Click += (sender, e) =>
        {
            ResetLevel();
        };

        panel.Children.Add(backButton);
        panel.Children.Add(levelInfo);
        panel.Children.Add(movesInfo);
        panel.Children.Add(resetButton);

        return panel;
    }

    private void ReturnToLevelSelection()
    {
        var selectLevelWindow = new SelectLevelWindow(levels, progInfo);
        selectLevelWindow.Show();
        Close();
    }

    private void ResetLevel()
    {
        var currentLevel = levels.FirstOrDefault(l => l.Name == session.Level.Name);
        if (currentLevel != null)
        {
            var newSession = new GameSession(currentLevel);
            session = newSession;
            frame.UpdateSession(newSession);
            session.Start();
            frame.InvalidateVisual();
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (pressedKeys.Add(e.Key))
        {
            lastPressedKey = e.Key;
            HandleInput();
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        pressedKeys.Remove(e.Key);
        if (lastPressedKey == e.Key)
            lastPressedKey = null;
    }

    private void HandleInput()
    {
        if (lastPressedKey is null)
            return;

        if (lastPressedKey == Key.R)
        {
            ResetLevel();
            return;
        }

        if (lastPressedKey == Key.Escape)
        {
            SaveManager.SaveLevelStats(progInfo.User, "Progress");
            ReturnToLevelSelection();
            return;
        }

        var direction = InputParser.ParseKey(lastPressedKey.Value);
        if (direction == Move.None)
            return;

        if (session.TryMove(direction))
        {
            frame.InvalidateVisual();
            if (session.IsGameOver && session.IsWin)
            {
                progInfo.User.AddSuccessfulAttempt(session.Level.Name,
                    session.ElapsedTime, session.MoveCount);
                ShowWinMessage();
            }
        }
    }

    private async void ShowWinMessage()
    {
        KeyDown -= OnKeyDown;
        KeyUp -= OnKeyUp;

        var dialog = new Window
        {
            Width = 320,
            Height = 180,
            Title = T("level is completed"),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
            SizeToContent = SizeToContent.Manual
        };

        var mainStackPanel = new StackPanel
        {
            Margin = new Thickness(20),
            Spacing = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var statsPanel = new StackPanel
        {
            Spacing = 8,
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var timeText = new TextBlock
        {
            Text = T("time") + $": {session.ElapsedTime:mm\\:ss}",
            FontSize = 16,
            FontWeight = FontWeight.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Foreground = Brushes.DarkBlue
        };

        var movesText = new TextBlock
        {
            Text = T("moves") + $": {session.MoveCount}",
            FontSize = 16,
            HorizontalAlignment = HorizontalAlignment.Center,
            Foreground = Brushes.DarkGreen
        };

        statsPanel.Children.Add(timeText);
        statsPanel.Children.Add(movesText);
        mainStackPanel.Children.Add(statsPanel);

        var buttonsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Spacing = 15
        };

        var backButton = new Button
        {
            Content = T("back to levels"),
            Padding = new Thickness(20, 10),
            FontSize = 14,
            MinWidth = 120,
            Background = Brushes.LightBlue,
            Foreground = Brushes.DarkBlue
        };

        var playAgainButton = new Button
        {
            Content = T("playAgain"),
            Padding = new Thickness(20, 10),
            FontSize = 14,
            MinWidth = 120,
            Background = Brushes.LightGreen,
            Foreground = Brushes.DarkGreen
        };

        backButton.Click += (s, e) =>
        {
            dialog.Close();
            SaveManager.SaveLevelStats(progInfo.User, "Progress");
            ReturnToLevelSelection();
        };

        playAgainButton.Click += (s, e) =>
        {
            dialog.Close();
            SaveManager.SaveLevelStats(progInfo.User, "Progress");
            ResetLevel();
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        };

        buttonsPanel.Children.Add(backButton);
        buttonsPanel.Children.Add(playAgainButton);
        mainStackPanel.Children.Add(buttonsPanel);

        dialog.Content = mainStackPanel;

        dialog.Closed += (s, e) =>
        {
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        };

        await dialog.ShowDialog(this);
    }

    private string T(string key) =>
        progInfo.Settings.Translations[progInfo.Settings.Language]
            .TryGetValue(key, out var value) ? value : key;
}