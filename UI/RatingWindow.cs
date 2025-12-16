using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Data;
using Sokoban.Models;
using Sokoban.Controller;
using System.Linq;
using System.Collections.Generic;

namespace Sokoban.UI;


public class RatingWindow : Window
{
    private ProgInfo progInfo;

    public RatingWindow(ProgInfo progInfo)
    {
        this.progInfo = progInfo;

        // Переведённый заголовок
        Title = T("rating");
        Width = progInfo.Settings.WindowWidth;
        Height = progInfo.Settings.WindowHeight;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        Content = CreateContent();
    }

    // Метод для перевода
    private string T(string key) =>
        progInfo.Settings.Translations[progInfo.Settings.Language]
            .TryGetValue(key, out var value) ? value : key;

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

        var rootGrid = new Grid();

        var users = LoadManager.LoadUsersProgress("Progress");

        var sortedUsers = users.OrderByDescending(u => u.LevelsStats.Count).ToList();

        int place = sortedUsers.FindIndex(u => u.Nickname == progInfo.User.Nickname) + 1;

        var placeText = new TextBlock
        {
            Text = string.Format(T("youAreInPlace"), place), // можно добавить ключ "youAreInPlace" в Translations
            FontSize = 24,
            FontWeight = FontWeight.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 20, 0, 10)
        };
        if (users.Any(u => u.Nickname == progInfo.User.Nickname))
            rootGrid.Children.Add(placeText);

        // Таблица
        var dataGrid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All,
            BorderThickness = new Thickness(1),
            BorderBrush = Brushes.Gray,
            Margin = new Thickness(20, 60, 20, 90)
        };

        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("place"),
            Binding = new Binding("Place"),
            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
        });
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("nickname"),
            Binding = new Binding("Nickname"),
            Width = new DataGridLength(2, DataGridLengthUnitType.Star)
        });
        dataGrid.Columns.Add(new DataGridTextColumn
        {
            Header = T("levelsCompleted"),
            Binding = new Binding("CompletedLevels"),
            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
        });

        var ratingItems = sortedUsers.Select((u, index) => new RatingItem
        {
            Place = index + 1,
            Nickname = u.Nickname,
            CompletedLevels = u.LevelsStats.Count
        }).ToList();

        dataGrid.ItemsSource = ratingItems;
        rootGrid.Children.Add(dataGrid);

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

        rootGrid.Children.Add(backButton);
        grid.Children.Add(rootGrid);

        return grid;
    }

    // Вспомогательный класс для таблицы
    private class RatingItem
    {
        public int Place { get; set; }
        public string Nickname { get; set; } = "";
        public int CompletedLevels { get; set; }
    }
}
