using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SavedSceneObject
	{
		public string ObjectID { get; set; }
		public string ModelPath { get; set; }
		public SimpleMaterial Material { get; set; }
		public SerializableTransform Transform { get; set; }

		public SimpleTransformAnimationComponent AnimationComponent { get; set; }
		public List<SceneObjectComponent> Components { get; set; }
	}
}
