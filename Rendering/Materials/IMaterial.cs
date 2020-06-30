using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public interface IMaterial : IDisposable
	{
		ShaderBytecode VertexShaderByteCode { get; }

		void Initialize(Device device);
		IMaterial CreateInstance();

		void Render(SceneObject sceneObject, Camera camera, Renderer renderer);
		void Render(Model model, Matrix transformMatrix, Camera camera, Renderer renderer);
	}
}
