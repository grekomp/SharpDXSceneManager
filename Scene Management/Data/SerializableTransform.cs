using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public struct SerializableTransform
	{
		public string TransformID { get; set; }
		public SerializableVector3 LocalPosition { get; set; }
		public SerializableVector3 LocalRotation { get; set; }
		public SerializableVector3 LocalScale { get; set; }
		public string ParentId { get; set; }
	}
}
