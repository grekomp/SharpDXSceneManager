using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	/// <summary>
	/// A simple animation component that allows for continuous linear animation of a transform.
	/// </summary>
	public class SimpleTransformAnimationComponent : SceneObjectComponent
	{
		private Vector3 rotationSpeed;
		private Vector3 movementSpeed;

		/// <summary>
		/// The rotation per second among xyz axes.
		/// </summary>
		public SerializableVector3 RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
		/// <summary>
		/// The movement per second vector.
		/// </summary>
		public SerializableVector3 MovementSpeed { get => movementSpeed; set => movementSpeed = value; }

		/// <summary>
		/// Performs the animation using the <see cref="RotationSpeed"/> and <see cref="MovementSpeed"/>.
		/// </summary>
		public override void Update()
		{
			SceneObject.transform.LocalRotation += rotationSpeed * Time.DeltaTime;
			SceneObject.transform.LocalPosition += movementSpeed * Time.DeltaTime;
		}
	}
}
