using System.Numerics;
using System.Runtime.InteropServices;
using FontStashSharp;
using FontStashSharp.Interfaces;
using Silk.NET.OpenGL;
using T_T_PandorasBox.Rendering.Buffers;
using Texture = T_T_PandorasBox.Rendering.Textures.Texture;

namespace T_T_PandorasBox.Rendering.Fonts;

public class FontRenderer: IFontStashRenderer2, IDisposable
	{
		
		private const int MAX_SPRITES = 2048;
		private const int MAX_VERTICES = MAX_SPRITES * 4;
		private const int MAX_INDICES = MAX_SPRITES * 6;

		private readonly Shader _shader;
		private readonly BufferObject<VertexPositionColorTexture> _vertexBuffer;
		private readonly BufferObject<short> _indexBuffer;
		private readonly VertexArrayObject<VertexPositionColorTexture, short> _vao;
		private readonly VertexPositionColorTexture[] _vertexData = new VertexPositionColorTexture[MAX_VERTICES];
		private object _lastTexture;
		private int _vertexIndex = 0;
		private readonly GL _gl;
		
		public ITexture2DManager TextureManager { get; }

		private static readonly short[] indexData = GenerateIndexArray();

		public unsafe FontRenderer(GL gl, ITexture2DManager textureManager)
		{
			_gl = gl;
			TextureManager = textureManager;
			
			_vertexBuffer = new BufferObject<VertexPositionColorTexture>(gl,MAX_VERTICES, BufferTargetARB.ArrayBuffer, true);
			_indexBuffer = new BufferObject<short>(gl, indexData.Length, BufferTargetARB.ElementArrayBuffer, false);
			_indexBuffer.SetData(indexData, 0, indexData.Length);
			
			_shader = new Shader(gl, "FontShader");
			_vao = new VertexArrayObject<VertexPositionColorTexture, short>(gl, _vertexBuffer, _indexBuffer);
			_vao.Bind();
			_vao.BindPointers(
				new VertexBinding(0, 3, false, VertexAttribPointerType.Float, 0),
				new VertexBinding(1, 4, true, VertexAttribPointerType.UnsignedByte, Marshal.SizeOf<Vector3>()),
				new VertexBinding(2, 2, false, VertexAttribPointerType.Float, Marshal.SizeOf<Vector3>() + Marshal.SizeOf<FSColor>())
			);
		}

		~FontRenderer() => Dispose();
		private bool _disposed = false;
		public void Dispose()
		{
			if(_disposed) return;
			_disposed = true;
			_vao.Dispose();
			_shader.Dispose();
		}

		public void Begin(Matrix4x4 matrix)
		{
			_shader.Use();
			_shader.SetMatrix("uModel", matrix);
			_shader.SetInt("TextureSampler", 0);
			_vao.Bind();
		}

		public void DrawQuad(object texture, ref VertexPositionColorTexture topLeft, ref VertexPositionColorTexture topRight, ref VertexPositionColorTexture bottomLeft, ref VertexPositionColorTexture bottomRight)
		{
			if (_lastTexture != texture)
			{
				FlushBuffer();
			}

			_vertexData[_vertexIndex++] = topLeft;
			_vertexData[_vertexIndex++] = topRight;
			_vertexData[_vertexIndex++] = bottomLeft;
			_vertexData[_vertexIndex++] = bottomRight;

			_lastTexture = texture;
		}

		public void End()
		{
			FlushBuffer();
		}

		private unsafe void FlushBuffer()
		{
			if (_vertexIndex == 0)
			{
				return;
			}

			_vertexBuffer.SetData(_vertexData, 0, _vertexIndex);

			var texture = (Texture)_lastTexture;
			texture.Bind();

			_gl.DrawElements(PrimitiveType.Triangles, (uint)(_vertexIndex * 6 / 4), DrawElementsType.UnsignedShort, null);
			_vertexIndex = 0;
		}

		private static short[] GenerateIndexArray()
		{
			short[] result = new short[MAX_INDICES];
			for (int i = 0, j = 0; i < MAX_INDICES; i += 6, j += 4)
			{
				result[i] = (short)(j);
				result[i + 1] = (short)(j + 1);
				result[i + 2] = (short)(j + 2);
				result[i + 3] = (short)(j + 3);
				result[i + 4] = (short)(j + 2);
				result[i + 5] = (short)(j + 1);
			}
			return result;
		}
	}