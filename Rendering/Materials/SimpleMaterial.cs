using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SceneManager
{
	public class SimpleMaterial : IMaterial
	{
		public struct VertexShaderData
		{
			public Matrix worldViewProj;
			public Matrix worldView;
			public Matrix world;
		}
		public struct PixelShaderData
		{
			public Vector4 lightPos;
			public Vector4 diffuseColor;
			public Vector4 emissionColor;
		}


		#region Shaders
		public string vertexShaderPath = "Shaders.fx";
		public string vertexShaderEntryPoint = "VS";
		public string vertexShaderProfile = "vs_5_0";

		public string pixelShaderPath = "Shaders.fx";
		public string pixelShaderEntryPoint = "PS";
		public string pixelShaderProfile = "ps_5_0";

		protected ShaderBytecode vertexShaderByteCode;
		public ShaderBytecode VertexShaderByteCode => vertexShaderByteCode;
		protected VertexShader vertexShader;

		protected ShaderBytecode pixelShaderByteCode;
		protected PixelShader pixelShader;

		protected Buffer vertexConstantBuffer;
		protected Buffer pixelConstantBuffer;

		protected Device device;
		#endregion


		#region Material data
		public VertexShaderData vertexShaderData;
		public PixelShaderData pixelShaderData;
		#endregion


		#region Initialization
		/// <summary>
		/// Creates a copy of the material with matching parameters
		/// </summary>
		public IMaterial CreateInstance()
		{
			return CreateInstanceTyped();
		}
		public SimpleMaterial CreateInstanceTyped()
		{
			SimpleMaterial copy = new SimpleMaterial();
			copy.vertexShaderPath = vertexShaderPath;
			copy.vertexShaderEntryPoint = vertexShaderEntryPoint;
			copy.vertexShaderProfile = vertexShaderProfile;
			copy.pixelShaderPath = pixelShaderPath;
			copy.pixelShaderEntryPoint = pixelShaderEntryPoint;
			copy.pixelShaderProfile = pixelShaderProfile;

			copy.vertexShaderData = vertexShaderData;
			copy.pixelShaderData = pixelShaderData;

			if (device != null)
			{
				copy.Initialize(device);
			}

			return copy;
		}

		public void Initialize(Device device)
		{
			this.device = device;
			CompileShaders(device);
			CreateConstantBuffers(device);
		}

		protected void CompileShaders(Device device)
		{
			vertexShaderByteCode = ShaderBytecode.CompileFromFile(vertexShaderPath, vertexShaderEntryPoint, vertexShaderProfile);
			vertexShader = new VertexShader(device, vertexShaderByteCode);

			pixelShaderByteCode = ShaderBytecode.CompileFromFile(pixelShaderPath, pixelShaderEntryPoint, pixelShaderProfile);
			pixelShader = new PixelShader(device, pixelShaderByteCode);
		}
		protected void CreateConstantBuffers(Device device)
		{
			vertexConstantBuffer = new Buffer(device, Utilities.SizeOf<VertexShaderData>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
			pixelConstantBuffer = new Buffer(device, Utilities.SizeOf<PixelShaderData>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 1);
		}
		#endregion


		#region Rendering
		public void Render(SceneObject sceneObject, Camera camera, Renderer renderer)
		{
			Render(sceneObject.model, sceneObject.transform.TransformMatrix, camera, renderer);
		}
		public void Render(Model model, Matrix transformMatrix, Camera camera, Renderer renderer)
		{
			if (device == null)
			{
				Initialize(renderer.device);
			}

			SetMatrices(transformMatrix, camera.ViewMatrix, camera.ProjMatrix);

			renderer.context.VertexShader.Set(vertexShader);
			renderer.context.PixelShader.Set(pixelShader);
			renderer.context.VertexShader.SetConstantBuffer(0, vertexConstantBuffer);
			renderer.context.PixelShader.SetConstantBuffer(0, pixelConstantBuffer);

			renderer.context.UpdateSubresource(ref pixelShaderData, pixelConstantBuffer);
			renderer.context.UpdateSubresource(ref vertexShaderData, vertexConstantBuffer);

			model.Render(renderer.context);
		}

		protected void SetMatrices(Matrix world, Matrix view, Matrix projection)
		{
			vertexShaderData.world = world;
			vertexShaderData.worldView = world * view;
			vertexShaderData.worldViewProj = vertexShaderData.worldView * projection;

			vertexShaderData.world.Transpose();
			vertexShaderData.worldView.Transpose();
			vertexShaderData.worldViewProj.Transpose();
		}
		#endregion


		#region Clean up
		public void Dispose()
		{
			vertexShaderByteCode.Dispose();
			vertexShader.Dispose();
			pixelShaderByteCode.Dispose();
			pixelShader.Dispose();
		}
		#endregion
	}
}
