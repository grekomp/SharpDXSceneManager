using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SceneManager
{
	public class SceneManager
	{
		public static Renderer currentRenderer;


		#region Saving scenes
		public static void SaveCurrentHierarchyRelative(string relativePath)
		{
			SaveCurrentHierarchy(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePath);
		}
		public static void SaveCurrentHierarchy(string basePath, string relativePath)
		{
			SaveCurrentHierarchyAbsolute(Path.Combine(basePath, relativePath));
		}
		public static void SaveCurrentHierarchyAbsolute(string absoluteSavePath)
		{
			SerializableScene serializableScene = new SerializableScene();

			foreach (var sceneObject in Hierarchy.sceneObjects)
			{
				serializableScene.SavedSceneObjects.Add(sceneObject.GetSerializableObject());
			}

			string jsonString = JsonSerializer.Serialize(serializableScene, new JsonSerializerOptions() { WriteIndented = true });
			Console.Write(jsonString);
			File.WriteAllText(absoluteSavePath, jsonString);
		}
		#endregion


		#region Loading scenes
		public static void LoadAndReplaceHierarchyRelative(string relativePath, Renderer renderer)
		{
			LoadAndReplaceHierarchy(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePath, renderer);
		}
		public static void LoadAndReplaceHierarchy(string basePath, string relativePath, Renderer renderer)
		{
			LoadAndReplaceHierarchyAbsolute(Path.Combine(basePath, relativePath), renderer);
		}
		public static void LoadAndReplaceHierarchyAbsolute(string absolutePath, Renderer renderer)
		{
			SceneManager.currentRenderer = renderer;

			string jsonString = File.ReadAllText(absolutePath);

			var scene = JsonSerializer.Deserialize<SerializableScene>(jsonString);

			Hierarchy.Dispose();

			foreach (var savedObject in scene.SavedSceneObjects)
			{
				Hierarchy.Add(new SceneObject(savedObject));
			}

			for (int i = 0; i < Hierarchy.sceneObjects.Count; i++)
			{
				if (scene.SavedSceneObjects[i].Transform.ParentId != null)
				{
					Hierarchy.sceneObjects[i].transform.parent = Hierarchy.sceneObjects.Find(so => so.transform.Id == scene.SavedSceneObjects[i].Transform.ParentId)?.transform;
				}
			}
		}
		#endregion




	}
}
