using System.Numerics;
using Api;
using Api.Game.Objects;
using Silk.NET.OpenGL;
using T_T_PandorasBox.Rendering.Fonts;
using T_T_PandorasBox.Rendering.Materials;
using T_T_PandorasBox.Rendering.Shapes;
using T_T_PandorasBox.Rendering.Textures;

namespace T_T_PandorasBox.Rendering
{
    internal unsafe class Renderer : IRenderer, IDisposable
    {
        private GL? _gl;
        private RectShape? _rectShape;
        private PlaneShape? _planeShape;
        private DynamicPlaneShape? _dynamicPlaneShape;
        private BasicShapeMaterial? _basicShapeMaterial;
        private CircleMaterial? _circleMaterial;
        private OutlineRectMaterial? _outlineRectMaterial;
        private int _screenWidth;
        private int _screenHeight;
        private Matrix4x4 _orthoMatrix;
        private readonly IGameCamera _gameCamera;

        private Texture2DManager? _texture2DManager;
        private FontRenderer? _fontRenderer;
        private Font? _font;
        private LineRenderer? _lineRenderer;

        public Renderer(IGameCamera gameCamera)
        {
            _gameCamera = gameCamera;
            _disposed = true;
        }

        public void Init(GL gl)
        {
            Dispose();
            _gl = gl;
            _rectShape = new RectShape(gl);
            _planeShape = new PlaneShape(gl);
            _dynamicPlaneShape = new DynamicPlaneShape(gl);
            _basicShapeMaterial = new BasicShapeMaterial(_gl);
            _circleMaterial = new CircleMaterial(_gl);
            _outlineRectMaterial = new OutlineRectMaterial(_gl);
            _lineRenderer = new LineRenderer(_gl);
            
            var viewport = new int[4];
            _gl.GetInteger(GetPName.Viewport, viewport);

             _screenWidth = viewport[2];
             _screenHeight = viewport[3];
             _orthoMatrix = Matrix4x4.CreateOrthographicOffCenter(0.0f, _screenWidth, _screenHeight, 0.0f, -1.0f, 1.0f);

             _texture2DManager = new Texture2DManager(_gl);
             _fontRenderer = new FontRenderer(_gl, _texture2DManager);
             _font = new Font(_fontRenderer, "Resources/Fonts/OpenSans-Regular.ttf");
             
             _disposed = false;
        }

        public void Unload()
        {
            Dispose();
            _rectShape = null;
            _planeShape = null;
            _dynamicPlaneShape = null;
            _basicShapeMaterial = null;
            _circleMaterial = null;
            _outlineRectMaterial = null;
            _texture2DManager = null;
            _fontRenderer = null;
            _font = null;
        }

        public void Rect(Vector2 position, Vector2 size, Color color)
        {
            if (_basicShapeMaterial is null || _rectShape is null) return;
            
            _basicShapeMaterial.Color = color;
            Render(position, size, _rectShape, _basicShapeMaterial);
        }
        
        public void Circle(Vector2 position, float size, Color color, float width, float time, float speed, int type)
        {
            if (_circleMaterial is null || _rectShape is null) return;

            if (width < 1)
            {
                width = 1;
            }
            
            _circleMaterial.FillColor = color;
            _circleMaterial.Size = size;
            _circleMaterial.Time = time;
            _circleMaterial.Width = width;
            _circleMaterial.Speed = speed;
            _circleMaterial.Type = type;
            
            Render(position, new Vector2(size + width/2, size + width/2), _rectShape, _circleMaterial);
        }
        
        private void Render(Vector2 position, Vector2 size, IShape shape, Material material)
        {
            if(_gl is null) return;
            
            var matrix = Matrix4x4.CreateScale(size.X, size.Y, 0) 
                         * Matrix4x4.CreateTranslation(position.X, position.Y, 0) 
                         * _orthoMatrix;
            
            shape.Vao.Bind();

            if (!material.Use(matrix))
            {
                return;
            }
            
            _gl.DrawElements(shape.PrimitiveType, (uint) shape.IndicesCount, DrawElementsType.UnsignedInt, null);
        }

        public void Rect3D(Vector3 start, Vector3 end, float width, Color color)
        {
            if (_outlineRectMaterial is null || _dynamicPlaneShape is null || _gl is null) return;
            
            _dynamicPlaneShape.Set(start, end, width);
            _outlineRectMaterial.FillColor = color;
            
            
            _dynamicPlaneShape.Vao.Bind();

            if (!_outlineRectMaterial.Use(_gameCamera.ViewProjMatrix))
            {
                return;
            }
            
            _gl.LineWidth(5.0f);
            _gl.DrawElements(PrimitiveType.LineLoop, (uint) _dynamicPlaneShape.IndicesCount, DrawElementsType.UnsignedInt, null);
        }
        
        public void Circle3D(Vector3 position, float size, Color color, float width, float time, float speed, int type)
        {
            if (_circleMaterial is null || _planeShape is null) return;
            
            if (width < 1)
            {
                width = 1;
            }
            
            _circleMaterial.FillColor = color;
            _circleMaterial.Size = size;
            _circleMaterial.Time = time;
            _circleMaterial.Width = width;
            _circleMaterial.Speed = speed;
            _circleMaterial.Type = type;
            Render3D(position, new Vector3(size + width/2, size + width/2, size + width/2), _planeShape, _circleMaterial);
        }
        
        private void Render3D(Vector3 position, Vector3 size,  IShape shape, Material material)
        {
            if(_gl is null) return;
            
            var translationMatrix = Matrix4x4.CreateTranslation(position);
            var scalingMatrix = Matrix4x4.CreateScale(size);
            var modelMatrix = scalingMatrix * translationMatrix; 
            var mvpMatrix = modelMatrix * _gameCamera.ViewProjMatrix;
            
            shape.Vao.Bind();

            if (!material.Use(mvpMatrix))
            {
                return;
            }
            
            _gl.DrawElements(shape.PrimitiveType, (uint) shape.IndicesCount, DrawElementsType.UnsignedInt, null);
        }
        
        public void Text(string text, Vector2 position, float size, Color color)
        {
            _font?.Render(text, position, size, color, _orthoMatrix);
        }
        
        public bool IsOnScreen(Vector2 position)
        {
            return position.X >= 0 && position.X <= _screenWidth && position.Y >= 0 && position.Y <= _screenHeight;
        }

        public void RenderLines(Vector3[] positions, float size, Color color)
        {
            _lineRenderer?.RenderLines(positions, size, color, _gameCamera.ViewProjMatrix);
        }
        
        public void RenderLines(IEnumerable<Vector3> positions, float size, Color color)
        {
            _lineRenderer?.RenderLines(positions.ToArray(), size, color, _gameCamera.ViewProjMatrix);
        }

        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (!_disposed) return;
            _disposed = true;
            _circleMaterial?.Dispose();
            _basicShapeMaterial?.Dispose();
            _outlineRectMaterial?.Dispose();
            _rectShape?.Dispose();
            _planeShape?.Dispose();
            _dynamicPlaneShape?.Dispose();
            _fontRenderer?.Dispose();
            _font?.Dispose();
            _lineRenderer?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Renderer()
        {
            Dispose(false);
        }
    }
}
