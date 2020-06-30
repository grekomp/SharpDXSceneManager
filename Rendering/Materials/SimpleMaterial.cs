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
using System.Text.Json.Serialization;

namespace SceneManager
{
	public class SimpleMaterial : IMaterial
	{
		public struct VertexShaderDataStruct
		{
			private Matrix worldViewProj;
			private Matrix worldView;
			private Matrix world;

			[JsonIgnore] public Matrix WorldViewProj { get => worldViewProj; set => worldViewProj = value; }
			[JsonIgnore] public Matrix WorldView { get => worldView; set => worldView = value; }
			[JsonIgnore] public Matrix World { get => world; set => world = value; }
		}
		public struct PixelShaderDataStruct
		{
			private Vector4 lightPos;
			private Vector4 diffuseColor;
			private Vector4 emissionColor;

			public SerializableVector4 LightPos { get => lightPos; set => lightPos = value; }
			public SerializableVector4 DiffuseColor { get => diffuseColor; set => diffuseColor = value; }
			public SerializableVector4 EmissionColor { get => emissionColor; set => emissionColor = value; }
		}


		#region Shaders
		private string vertexShaderPath = "Shaders.fx";
		private string vertexShaderEntryPoint = "VS";
		private string vertexShaderProfile = "vs_5_0";

		private string pixelShaderPath = "Shaders.fx";
		private string pixelShaderEntryPoint = "PS";
		private string pixelShaderProfile = "ps_5_0";

		public string VertexShaderPath { get => vertexShaderPath; set => vertexShaderPath = value; }
		public string VertexShaderEntryPoint { get => vertexShaderEntryPoint; set => vertexShaderEntryPoint = value; }
		public string VertexShaderProfile { get => vertexShaderProfile; set => vertexShaderProfile = value; }
		public string PixelShaderPath { get => pixelShaderPath; set => pixelShaderPath = value; }
		public string PixelShaderEntryPoint { get => pixelShaderEntryPoint; set => pixelShaderEntryPoint = value; }
		public string PixelShaderProfile { get => pixelShaderProfile; set => pixelShaderProfile = value; }


		protected ShaderBytecode vertexShaderByteCode;
		[JsonIgnore]
		public ShaderBytecode VertexShaderByteCode => vertexShaderByteCode;

		protected VertexShader vertexShader;

		protected ShaderBytecode pixelShaderByteCode;
		protected PixelShader pixelShader;

		protected Buffer vertexConstantBuffer;
		protected Buffer pixelConstantBuffer;

		protected Device device;
		#endregion


		#region Material data
		public VertexShaderDataStruct vertexShaderData;
		public PixelShaderDataStruct pixelShaderData;

		[JsonIgnore] public VertexShaderDataStruct VertexShaderData { get => vertexShaderData; set => vertexShaderData = value; }
		public PixelShaderDataStruct PixelShaderData { get => pixelShaderData; set => pixelShaderData = value; }
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
			copy.VertexShaderPath = VertexShaderPath;
			copy.VertexShaderEntryPoint = VertexShaderEntryPoint;
			copy.VertexShaderProfile = VertexShaderProfile;
			copy.PixelShaderPath = PixelShaderPath;
			copy.PixelShaderEntryPoint = PixelShaderEntryPoint;
			copy.PixelShaderProfile = PixelShaderProfile;

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
			vertexShaderByteCode = ShaderBytecode.CompileFromFile(VertexShaderPath, VertexShaderEntryPoint, VertexShaderProfile);
			vertexShader = new VertexShader(device, vertexShaderByteCode);

			pixelShaderByteCode = ShaderBytecode.CompileFromFile(PixelShaderPath, PixelShaderEntryPoint, PixelShaderProfile);
			pixelShader = new PixelShader(device, pixelShaderByteCode);
		}
		protected void CreateConstantBuffers(Device device)
		{
			vertexConstantBuffer = new Buffer(device, Utilities.SizeOf<VertexShaderDataStruct>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
			pixelConstantBuffer = new Buffer(device, Utilities.SizeOf<PixelShaderDataStruct>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 1);
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
			vertexShaderData.World = world;
			vertexShaderData.WorldView = world * view;
			vertexShaderData.WorldViewProj = vertexShaderData.WorldView * projection;

			vertexShaderData.World.Transpose();
			vertexShaderData.WorldView.Transpose();
			vertexShaderData.WorldViewProj.Transpose();
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
