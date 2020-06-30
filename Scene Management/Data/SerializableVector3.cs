using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneManager
{
	public struct SerializableVector3
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }


		public SerializableVector3(Vector3 vector)
		{
			X = vector.X;
			Y = vector.Y;
			Z = vector.Z;
		}

		public static implicit operator SerializableVector3(Vector3 vector) => new SerializableVector3(vector);
		public static implicit operator Vector3(SerializableVector3 serializableVector) => new Vector3(serializableVector.X, serializableVector.Y, serializableVector.Z);
	}
}
