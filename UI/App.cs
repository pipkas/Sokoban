using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Sokoban.Controller;
using Avalonia.Controls.ApplicationLifetimes;
using Sokoban.Models;

namespace Sokoban.UI;
public class App : Application
{
    public override void Initialize()
    {
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new StartWindow(new Settings());
        }
        base.OnFrameworkInitializationCompleted();
    }
}

