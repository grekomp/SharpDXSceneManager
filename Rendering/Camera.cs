using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public class Camera
	{
		public Vector3 cameraPosition;
		public Vector3 cameraLookAt;

		public float fov = (float)Math.PI / 4.0f;
		public float aspectRatio = 16f / 9f;
		public float zNear = 0.1f;
		public float zFar = 100f;

		public Matrix ViewMatrix => Matrix.LookAtLH(cameraPosition, cameraLookAt, Vector3.UnitY);
		public Matrix ProjMatrix => Matrix.PerspectiveFovLH(fov, aspectRatio, zNear, zFar);
	}
}
