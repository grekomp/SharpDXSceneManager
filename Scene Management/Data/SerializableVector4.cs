using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneManager
{
	public struct SerializableVector4
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float W { get; set; }

		public SerializableVector4(Vector4 vector)
		{
			X = vector.X;
			Y = vector.Y;
			Z = vector.Z;
			W = vector.W;
		}

		public static implicit operator SerializableVector4(Vector4 vector) => new SerializableVector4(vector);
		public static implicit operator Vector4(SerializableVector4 serializableVector) => new Vector4(serializableVector.X, serializableVector.Y, serializableVector.Z, serializableVector.W);
	}
}
