﻿using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.D3DCompiler;
using AssimpWrapper;

using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace SceneManager
{
	/// <summary>
	/// A container for meshes loaded from a file.
	/// </summary>
	public class Model
	{
		/// <summary>
		/// The base (application) path that was used when loading this model.
		/// </summary>
		public string basePath;
		/// <summary>
		/// The relative file path used when loading this model.
		/// </summary>
		public string modelPath;

		List<ModelMesh> m_meshes;
		bool m_inputLayoutSet;

		Vector3 m_aaBoxMin;
		/// <summary>
		/// Bounding box corner.
		/// </summary>
		public Vector3 AABoxMin {
			set { m_aaBoxMin = value; }
			get { return m_aaBoxMin; }
		}

		Vector3 m_aaBoxMax;
		/// <summary>
		/// Second bounding box corner.
		/// </summary>
		public Vector3 AABoxMax {
			set { m_aaBoxMax = value; }
			get { return m_aaBoxMax; }
		}

		Vector3 m_aaBoxCentre;
		/// <summary>
		/// The center of the bounding box.
		/// </summary>
		public Vector3 AABoxCentre {
			set { m_aaBoxCentre = value; }
			get { return m_aaBoxCentre; }
		}

		/// <summary>
		/// Initializes a new instance of the Model class.
		/// </summary>
		public Model()
		{
			m_meshes = new List<ModelMesh>();
			m_inputLayoutSet = false;
		}

		/// <summary>
		/// Adds a mesh to this model.
		/// </summary>
		/// <param name="mesh">The mesh to be added to this model.</param>
		public void AddMesh(ref ModelMesh mesh)
		{
			m_meshes.Add(mesh);
		}

		/// <summary>
		/// Sets the bounding box coordinates.
		/// </summary>
		/// <param name="min">The first bounding box corner.</param>
		/// <param name="max">The second bounding box corner.</param>
		public void SetAABox(Vector3 min, Vector3 max)
		{
			m_aaBoxMin = min;
			m_aaBoxMax = max;
			m_aaBoxCentre = 0.5f * (min + max);
		}

		/// <summary>
		/// Render the model using the currently configured vertex and pixel shaders. 
		/// This method should not be used when rendering scene objects, use <see cref="SceneObject.Render(Camera, Renderer)"/> instead.
		/// </summary>
		/// <param name="context">The DirectX device context to be used for rendering.</param>
		public void Render(DeviceContext context)
		{
			if (!m_inputLayoutSet)
				throw new Exception("Model::Render(): input layout has not be specified, you must call SetInputLayout() before calling Render()");

			foreach (ModelMesh mesh in m_meshes)
			{
				//set mesh specific data
				context.InputAssembler.InputLayout = mesh.InputLayout;
				context.InputAssembler.PrimitiveTopology = mesh.PrimitiveTopology;
				context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(mesh.VertexBuffer, mesh.VertexSize, 0));
				context.InputAssembler.SetIndexBuffer(mesh.IndexBuffer, Format.R32_UInt, 0);
				context.PixelShader.SetShaderResource(0, mesh.DiffuseTextureView);

				//draw
				context.DrawIndexed(mesh.IndexCount, 0, 0);
			}
		}

		/// <summary>
		/// Sets the vertex data input signature according to the shader signature.
		/// </summary>
		/// <param name="device">The DirectX device used for rendering.</param>
		/// <param name="inputSignature">The target shader input signature.</param>
		public void SetInputLayout(Device device, ShaderSignature inputSignature)
		{
			foreach (ModelMesh mesh in m_meshes)
			{
				mesh.SetInputLayout(device, inputSignature);
			}
			m_inputLayoutSet = true;
		}

		/// <summary>
		/// Releases all contained meshes.
		/// </summary>
		public void Dispose()
		{
			foreach (ModelMesh mesh in m_meshes)
			{
				mesh.Dispose();
			}
		}

	}
}
