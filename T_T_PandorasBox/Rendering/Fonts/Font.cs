using System.Numerics;
using Api;
using FontStashSharp;

namespace T_T_PandorasBox.Rendering.Fonts;


public class Font : IDisposable
{
    private readonly FontSystem _fontSystem;
    private readonly FontRenderer _fontRenderer;
    public Font(FontRenderer fontRenderer, params string[] fontPaths)
    {
        _fontRenderer = fontRenderer;
        var settings = new FontSystemSettings
        {
            FontResolutionFactor = 2,
            KernelWidth = 2,
            KernelHeight = 2
        };

        _fontSystem = new FontSystem(settings);

        foreach (var fontPath in fontPaths)
        {
            _fontSystem.AddFont(File.ReadAllBytes(fontPath));
        }
        
        _fontSystem.AddFont(File.ReadAllBytes("Resources/Fonts/DroidSans.ttf"));
        _fontSystem.AddFont(File.ReadAllBytes("Resources/Fonts/DroidSansJapanese.ttf"));
        _fontSystem.AddFont(File.ReadAllBytes("Resources/Fonts/Symbola-Emoji.ttf"));
    }

    public void Render(string text, Vector2 position, float fontSize, Color color, Matrix4x4 matrix)
    {
        var font = _fontSystem.GetFont(fontSize);
        var scale = new Vector2(1, 1);
        var size = font.MeasureString(text, scale);
        var origin = new Vector2(size.X / 2.0f, size.Y / 2.0f);
        
        _fontRenderer.Begin(matrix);
        font.DrawText(_fontRenderer, text, position, new FSColor(color.R, color.G, color.B, color.A), scale, 0, origin);
        _fontRenderer.End();
    }

    public void Dispose()
    {
        _fontSystem.Dispose();
    }
}