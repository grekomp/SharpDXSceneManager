using SharpDX;
using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SceneObject : IDisposable
	{
		public string Id { get; private set; }

		public bool isActive = true;

		public Model model;
		public IMaterial material;
		public Transform transform = new Transform();

		public List<SceneObjectComponent> components = new List<SceneObjectComponent>();


		#region Initialization
		public SceneObject()
		{
			Id = Guid.NewGuid().ToString();
			Hierarchy.Add(this);
		}
		public SceneObject(SavedSceneObject savedSceneObject)
		{
			Id = savedSceneObject.objectID;

			// TODO: Load Model

			material = savedSceneObject.material;
			transform = new Transform(savedSceneObject.transform);

			foreach (var component in savedSceneObject.components)
			{
				AddComponent(component);
			}
		}
		#endregion


		#region Executing components
		/// <summary>
		/// Runs the Update method on all components.
		/// 
		/// Should be executed every frame before any rendering.
		/// </summary>
		public void Update()
		{
			foreach (var component in components)
			{
				component.Update();
			}
		}

		public void AddComponent(SceneObjectComponent component)
		{
			component.BindSceneObject(this);
			components.Add(component);
		}
		#endregion


		#region Rendering
		public void Render(Camera camera, Renderer renderer)
		{
			if (model != null && material != null)
			{
				model.SetInputLayout(renderer.device, ShaderSignature.GetInputSignature(material.VertexShaderByteCode));
				material.Render(this, camera, renderer);
			}
		}
		#endregion


		#region Serialization
		public SavedSceneObject GetSerializableObject()
		{
			var serializableObject = new SavedSceneObject();

			serializableObject.objectID = Id;
			serializableObject.modelPath = model != null ? model.modelPath : "";
			serializableObject.material = material as SimpleMaterial;
			serializableObject.transform = transform.GetSerializableTransform();

			serializableObject.components = new List<SceneObjectComponent>(components);

			return serializableObject;
		}
		#endregion


		#region Clean up
		public void Dispose()
		{
			model?.Dispose();
			material?.Dispose();

			foreach (var component in components)
			{
				component.Dispose();
			}
		}
		#endregion
	}
}
