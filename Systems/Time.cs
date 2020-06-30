using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SceneManager
{
	public static class Time
	{
		private static float deltaTime;
		private static float elapsed;
		private static float lastFrameElapsed;

		private static Stopwatch stopwatch;

		public static float DeltaTime { get => deltaTime; private set => deltaTime = value; }
		public static float Elapsed { get => elapsed; set => elapsed = value; }


		public static void Start()
		{
			stopwatch = new Stopwatch();
			stopwatch.Start();
		}
		public static void NextFrame()
		{
			lastFrameElapsed = elapsed;
			elapsed = stopwatch.ElapsedMilliseconds / 1000f;
			deltaTime = elapsed - lastFrameElapsed;
		}
		public static void ResetTime()
		{
			stopwatch.Restart();
			lastFrameElapsed = 0;
			elapsed = 0;
		}
	}
}
