using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class Hierarchy
	{
		public static List<SceneObject> sceneObjects = new List<SceneObject>();

		private static List<SceneObject> objectsScheduledForDeletion = new List<SceneObject>();

		#region Managing sceneObjects
		public static void Add(SceneObject sceneObject)
		{
			sceneObjects.Add(sceneObject);
		}
		public static void Remove(SceneObject sceneObject)
		{
			ScheduleForDeletion(sceneObject);
			RemoveChildrenRecursively(sceneObject.transform);
			DeleteScheduledObjects();
		}

		private static void RemoveChildrenRecursively(Transform parent)
		{
			foreach (var sceneObject in sceneObjects)
			{
				if (sceneObject.transform.parent == parent)
				{
					ScheduleForDeletion(sceneObject);
					RemoveChildrenRecursively(sceneObject.transform);
				}
			}
		}

		private static void ScheduleForDeletion(SceneObject sceneObject)
		{
			if (sceneObjects.Contains(sceneObject))
			{
				objectsScheduledForDeletion.Add(sceneObject);
				RemoveChildrenRecursively(sceneObject.transform);
				sceneObject.Dispose();
			}
		}
		private static void DeleteScheduledObjects()
		{
			foreach (var sceneObject in objectsScheduledForDeletion)
			{
				if (sceneObject == null) continue;

				if (sceneObjects.Contains(sceneObject)) sceneObjects.Remove(sceneObject);

				sceneObject.Dispose();
			}

			objectsScheduledForDeletion.Clear();
		}
		#endregion


		#region Executing events
		public static void Update()
		{
			UpdateChildrenRecursively(null);
		}
		protected static void UpdateChildrenRecursively(Transform parent)
		{
			foreach (var sceneObject in sceneObjects)
			{
				if (sceneObject.isActive && sceneObject.transform.parent == parent)
				{
					sceneObject.Update();
					UpdateChildrenRecursively(sceneObject.transform);
				}
			}
		}
		#endregion


		#region Rendering objects
		public static void Render(Camera camera, Renderer renderer)
		{
			RenderChildrenRecursively(camera, renderer, null);
		}
		private static void RenderChildrenRecursively(Camera camera, Renderer renderer, Transform parent)
		{
			foreach (var sceneObject in sceneObjects)
			{
				if (sceneObject.isActive && sceneObject.transform.parent == parent)
				{
					sceneObject.Render(camera, renderer);
					RenderChildrenRecursively(camera, renderer, sceneObject.transform);
				}
			}
		}
		#endregion
	}
}
