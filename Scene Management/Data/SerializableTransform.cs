using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public struct SerializableTransform
	{
		public string transformID;
		public Vector3 localPosition;
		public Vector3 localRotation;
		public Vector3 localScale;
		public string parentId;
	}
}
