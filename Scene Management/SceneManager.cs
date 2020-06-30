using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SceneManager
{
	/// <summary>
	/// Handles saving and loading scene .json files from disc.
	/// </summary>
	public class SceneManager
	{
		/// <summary>
		/// The renderer used when initializing objects loaded from scene files.
		/// </summary>
		public static Renderer currentRenderer;


		#region Saving scenes
		/// <summary>
		/// Saves the current scene object hierarchy to a scene file under the specified relativePath, relative to the executable file.
		/// </summary>
		/// <param name="relativePath">Path and filename to save the scene to, relative to the executable.</param>
		public static void SaveCurrentHierarchyRelative(string relativePath)
		{
			SaveCurrentHierarchy(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePath);
		}
		/// <summary>
		/// Saves the current scene object hierarchy to a scene file under the specified relativePath, relative to the basePath.
		/// </summary>
		/// <param name="basePath">The base path for loading the scene file.</param>
		/// <param name="relativePath">Path and filename to save the scene to, relative to the basePath.</param>
		public static void SaveCurrentHierarchy(string basePath, string relativePath)
		{
			SaveCurrentHierarchyAbsolute(Path.Combine(basePath, relativePath));
		}
		/// <summary>
		/// Saves the current scene object hierarchy to a scene file under the specified path.
		/// </summary>
		/// <param name="absoluteSavePath">Absolute path to save the scene file to.</param>
		public static void SaveCurrentHierarchyAbsolute(string absoluteSavePath)
		{
			SerializableScene serializableScene = new SerializableScene();

			foreach (var sceneObject in Hierarchy.sceneObjects)
			{
				serializableScene.SavedSceneObjects.Add(sceneObject.GetSerializableObject());
			}

			string jsonString = JsonSerializer.Serialize(serializableScene, new JsonSerializerOptions() { WriteIndented = true });
			//Console.Write(jsonString);
			File.WriteAllText(absoluteSavePath, jsonString);
		}
		#endregion


		#region Loading scenes
		/// <summary>
		/// Loads the scene file from the specified file, relative to the executable path, discards current hierarchy and configures the loaded scene objects using the provided renderer.
		/// </summary>
		/// <param name="relativePath">The path to the scene file.</param>
		/// <param name="renderer">The renderer to use when configuring the loaded scene objects.</param>
		public static void LoadAndReplaceHierarchyRelative(string relativePath, Renderer renderer)
		{
			LoadAndReplaceHierarchy(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), relativePath, renderer);
		}
		/// <summary>
		/// Loads the scene file from the specified file, relative to the basePath, discards current hierarchy and configures the loaded scene objects using the provided renderer.
		/// </summary>
		/// <param name="relativePath">The path to the scene file.</param>
		/// <param name="renderer">The renderer to use when configuring the loaded scene objects.</param>
		public static void LoadAndReplaceHierarchy(string basePath, string relativePath, Renderer renderer)
		{
			LoadAndReplaceHierarchyAbsolute(Path.Combine(basePath, relativePath), renderer);
		}
		/// <summary>
		/// Loads the scene file from the specified file, discards current hierarchy and configures the loaded scene objects using the provided renderer.
		/// </summary>
		/// <param name="absolutePath">Absolute path to the scene file.</param>
		/// <param name="renderer">The renderer to use when configuring the loaded scene objects.</param>
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
