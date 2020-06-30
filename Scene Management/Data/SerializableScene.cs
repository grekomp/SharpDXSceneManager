using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	[Serializable]
	public class SerializableScene
	{
		public string SceneName { get; set; }
		public List<SavedSceneObject> SavedSceneObjects { get; set; }


		public SerializableScene()
		{
			SceneName = "Unnamed Scene";
			SavedSceneObjects = new List<SavedSceneObject>();
		}
	}
}
