using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SavedSceneObject
	{
		public string objectID;
		public string modelPath;
		public SimpleMaterial material;
		public SerializableTransform transform;

		public List<SceneObjectComponent> components = new List<SceneObjectComponent>();
	}
}
