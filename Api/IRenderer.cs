using System.Numerics;
using Silk.NET.OpenGL;

namespace Api
{
    public interface IRenderer
    {
        void Init(GL gl);
        void Unload();
        void Rect(Vector2 position, Vector2 size, Color color);
        void Circle(Vector2 position, float size, Color color, float width, float time, float speed, int type);
        void Rect3D(Vector3 start, Vector3 end, float width, Color color);
        void Circle3D(Vector3 position, float size, Color color, float width, float time, float speed, int type);

        void Text(string text, Vector2 position, float size, Color color);
        
        bool IsOnScreen(Vector2 position);
        void RenderLines(Vector3[] positions, float size, Color color);
        void RenderLines(IEnumerable<Vector3> positions, float size, Color color);
    }
}