using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Sokoban.Models;

namespace Sokoban.UI;

public class Frame : UserControl
{
    private readonly Dictionary<CellType, Bitmap> _bitmaps = new();
    private GameSession session;
    private readonly ProgInfo progInfo;

    public Frame(GameSession session, ProgInfo progInfo)
    {
        this.session = session;
        this.progInfo = progInfo;
        LoadImages();
    }

    private void LoadImages()
    {
        var imagesDir = new DirectoryInfo("Images");
        var cellTypeToFile = new Dictionary<CellType, string>
        {
            { CellType.Empty, "empty.png" },
            { CellType.Wall, "wall.png" },
            { CellType.Box, "box.png" },
            { CellType.Target, "target.png" },
            { CellType.BoxInTarget, "boxInTarget.png" },
        };

        foreach (var file in imagesDir.GetFiles("*.png"))
        {
            var bitmap = new Bitmap(file.FullName);
            var cellType = cellTypeToFile.FirstOrDefault(x => x.Value == file.Name).Key;
            if (cellType != default) _bitmaps[cellType] = bitmap;
        }

        _bitmaps[CellType.Player] = new Bitmap(Path.Combine(imagesDir.FullName, progInfo.User.PlayerImage));
    }

    public void UpdateSession(GameSession newSession)
    {
        this.session = newSession;
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        if (session.Level == null) return;

        var level = session.Level;
        var cellSize = progInfo.Settings.GameCellSize;

        for (int x = 0; x < level.Width; x++)
        {
            for (int y = 0; y < level.Height; y++)
            {
                var pos = new CellPos(x, y);
                var cell = level.GetCell(pos) ?? CellType.Empty;
                var rect = new Rect(x * cellSize, y * cellSize, cellSize, cellSize);

                if (_bitmaps.TryGetValue(cell, out var bitmap))
                    context.DrawImage(bitmap, rect);
                else
                    context.FillRectangle(Brushes.Black, rect);
            }
        }

        var playerRect = new Rect(session.Player.Position.X * cellSize,
                                  session.Player.Position.Y * cellSize, cellSize, cellSize);

        if (_bitmaps.TryGetValue(CellType.Player, out var playerBitmap))
            context.DrawImage(playerBitmap, playerRect);
    }
}