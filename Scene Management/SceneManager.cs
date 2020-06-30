using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SceneManager
{
	public class SceneManager
	{
		public static void SaveCurrentHierarchy(string savePath)
		{
			SerializableScene serializableScene = new SerializableScene();

			foreach (var sceneObject in Hierarchy.sceneObjects)
			{
				serializableScene.SavedSceneObjects.Add(sceneObject.GetSerializableObject());
			}

			Console.Write(JsonSerializer.Serialize(serializableScene, new JsonSerializerOptions() { WriteIndented = true }));
		}
	}
}
