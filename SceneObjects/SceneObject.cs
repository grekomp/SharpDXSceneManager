using AssimpWrapper;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
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
			Id = savedSceneObject.ObjectID;

			if (string.IsNullOrEmpty(savedSceneObject.ModelPath) == false) model = new ModelLoader(SceneManager.currentRenderer.device).LoadRelative(savedSceneObject.ModelPath);
			material = savedSceneObject.Material;
			transform = new Transform(savedSceneObject.Transform);

			foreach (var component in savedSceneObject.Components)
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
				material.Render(this, camera, renderer);
			}
		}
		#endregion


		#region Serialization
		public SavedSceneObject GetSerializableObject()
		{
			var serializableObject = new SavedSceneObject();

			serializableObject.ObjectID = Id;
			serializableObject.ModelPath = model != null ? model.modelPath : "";
			serializableObject.Material = material as SimpleMaterial;
			serializableObject.Transform = transform.GetSerializableTransform();

			serializableObject.Components = new List<SceneObjectComponent>(components);

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
