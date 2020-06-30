using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SimpleTransformAnimationComponent : SceneObjectComponent
	{
		private Vector3 rotationSpeed;
		private Vector3 movementSpeed;

		public SerializableVector3 RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
		public SerializableVector3 MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

		public override void Update()
		{
			SceneObject.transform.LocalRotation += rotationSpeed * Time.DeltaTime;
			SceneObject.transform.LocalPosition += movementSpeed * Time.DeltaTime;
		}
	}
}
