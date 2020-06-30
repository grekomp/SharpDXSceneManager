using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SerializableScene
	{
		public string sceneName = "Unnamed Scene";

		public List<SavedSceneObject> savedSceneObjects = new List<SavedSceneObject>();
	}
}
