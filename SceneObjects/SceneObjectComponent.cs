using System;
using System.Text.Json.Serialization;

namespace SceneManager
{
	public class SceneObjectComponent
	{
		[JsonIgnore]
		public SceneObject SceneObject { get; protected set; }

		public void BindSceneObject(SceneObject sceneObject)
		{
			if (SceneObject == null)
			{
				SceneObject = sceneObject;
			}
			else
			{
				Console.WriteLine("ERROR: {0}: Cannot bind SceneObjectComponent, the component was already bound to a SceneObject.");
			}
		}

		/// <summary>
		/// Called every frame before any rendering.
		/// </summary>
		public virtual void Update()
		{
			if (SceneObject == null)
			{
				Console.WriteLine("ERROR: {0}: Cannot execute SceneObjectComponent updates without a bound scene object.", this);
			}
		}

		public virtual void Dispose() { }
	}
}