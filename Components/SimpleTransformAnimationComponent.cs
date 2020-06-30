using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class SimpleTransformAnimationComponent : SceneObjectComponent
	{
		public Vector3 rotationSpeed;
		public Vector3 movementSpeed;

		public override void Update()
		{
			SceneObject.transform.LocalRotation += rotationSpeed * Time.DeltaTime;
			SceneObject.transform.LocalPosition += movementSpeed * Time.DeltaTime;
		}
	}
}
