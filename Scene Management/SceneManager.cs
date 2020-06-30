using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SceneManager
	{
		public static void SaveCurrentHierarchy(string savePath)
		{
			List<SavedSceneObject> serializableSceneObjects = new List<SavedSceneObject>();

			foreach (var sceneObject in Hierarchy.sceneObjects)
			{
				serializableSceneObjects.Add(sceneObject.GetSerializableObject());
			}


		}
	}
}
