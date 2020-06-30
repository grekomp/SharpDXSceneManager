using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class Transform
	{
		private Vector3 localPosition;
		private Vector3 localRotation;
		private Vector3 localScale;

		private Matrix transformMatrix;
		private bool isDirty;

		public Transform parent;


		#region Properties
		public string Id { get; private set; }

		public Vector3 LocalPosition {
			get => localPosition;
			set {
				localPosition = value;
				isDirty = true;
			}
		}
		public Vector3 LocalRotation {
			get => localRotation;
			set {
				localRotation = value;
				isDirty = true;
			}
		}
		public Vector3 LocalScale {
			get => localScale;
			set {
				localScale = value;
				isDirty = true;
			}
		}
		public float LocalPositionX { get => localPosition.X; set => localPosition.X = value; }
		public float LocalPositionY { get => localPosition.Y; set => localPosition.Y = value; }
		public float LocalPositionZ { get => localPosition.Z; set => localPosition.Z = value; }

		public float LocalRotationX { get => localRotation.X; set => localRotation.X = value; }
		public float LocalRotationY { get => localRotation.Y; set => localRotation.Y = value; }
		public float LocalRotationZ { get => localRotation.Z; set => localRotation.Z = value; }

		public float LocalScaleX { get => localScale.X; set => localScale.X = value; }
		public float LocalScaleY { get => localScale.Y; set => localScale.Y = value; }
		public float LocalScaleZ { get => localScale.Z; set => localScale.Z = value; }

		public Matrix TransformMatrix {
			get {
				if (isDirty)
				{
					transformMatrix = Matrix.Transformation(Vector3.Zero, Quaternion.Identity, localScale, Vector3.Zero, Quaternion.RotationYawPitchRoll(LocalRotationX, LocalRotationY, LocalRotationZ), localPosition);
				}

				if (parent != null)
				{
					return transformMatrix * parent.TransformMatrix;
				}

				return transformMatrix;
			}
		}

		public bool IsDirty {
			get {
				return parent != null ? isDirty || parent.IsDirty : isDirty;
			}
			private set => isDirty = value;
		}
		#endregion


		#region Initialization
		public Transform()
		{
			Id = Guid.NewGuid().ToString();

			LocalPosition = Vector3.Zero;
			LocalRotation = Vector3.Zero;
			LocalScale = Vector3.One;
		}
		public Transform(SerializableTransform serializableTransform)
		{
			Id = serializableTransform.TransformID;
			LocalPosition = serializableTransform.LocalPosition;
			LocalRotation = serializableTransform.LocalRotation;
			LocalScale = serializableTransform.LocalScale;

		}
		#endregion


		#region Serialization
		public SerializableTransform GetSerializableTransform()
		{
			SerializableTransform serializableTransform = new SerializableTransform();
			serializableTransform.TransformID = Id;
			serializableTransform.ParentId = parent?.Id;

			serializableTransform.LocalPosition = localPosition;
			serializableTransform.LocalRotation = localRotation;
			serializableTransform.LocalScale = localScale;

			return serializableTransform;
		}
		#endregion
	}
}
