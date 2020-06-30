// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using AssimpWrapper;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace SceneManager
{
	/// <summary>
	/// SharpDX MiniCubeTexture Direct3D 11 Sample
	/// </summary>
	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			float speed = 100f;
			float orbitDistanceMultiplier = 1.5f;
			float planetScaleMultiplier = 0.2f;

			// Create Renderer
			Renderer renderer = new Renderer("SharpDX - SceneManager", 1280, 720, 60);

			SceneManager.LoadAndReplaceHierarchyRelative("Earth and moon.json", renderer);

			//// Create and setup material
			//SimpleMaterial baseMaterial = new SimpleMaterial
			//{
			//	VertexShaderPath = "Shaders.fx",
			//	VertexShaderEntryPoint = "VS",
			//	VertexShaderProfile = "vs_5_0",
			//	PixelShaderPath = "Shaders.fx",
			//	PixelShaderEntryPoint = "PS",
			//	PixelShaderProfile = "ps_5_0",
			//};
			//baseMaterial.Initialize(renderer.device);
			//baseMaterial.pixelShaderData.LightPos = new Vector4(0, 0, 0, 1);
			//baseMaterial.pixelShaderData.EmissionColor = new Vector4(0.1f, 0.1f, 0.1f, 1);


			//// Load model
			//ModelLoader modelLoader = new ModelLoader(renderer.device);
			//Model sphere = modelLoader.LoadRelative("Assets\\Planets\\sphere.obj");

			//// Create scene objects

			//// Sun
			//SceneObject sunObject = new SceneObject();
			//sunObject.model = sphere;
			//SimpleMaterial sunMaterial = baseMaterial.CreateInstanceTyped();
			//sunMaterial.pixelShaderData.DiffuseColor = new Vector4(0.92f, 0.72f, 0.2f, 1);
			//sunMaterial.pixelShaderData.EmissionColor = new Vector4(0.92f, 0.8f, 0.2f, 1);
			//sunObject.material = sunMaterial;

			//// Mercury
			//SceneObject mercuryRoot = new SceneObject();
			//mercuryRoot.AddComponent(new SimpleTransformAnimationComponent()
			//{
			//	RotationSpeed = new Vector3(4f * speed, 0, 0),
			//});
			//SceneObject mercury = new SceneObject();
			//mercury.model = sphere;
			//mercury.transform.LocalPositionX = 0.36f * orbitDistanceMultiplier;
			//mercury.transform.LocalScale = Vector3.One * 0.38f * planetScaleMultiplier;
			//mercury.transform.parent = mercuryRoot.transform;
			//SimpleMaterial mercuryMaterial = baseMaterial.CreateInstanceTyped();
			//mercuryMaterial.pixelShaderData.DiffuseColor = new Vector4(0.7f, 0.6f, 0.5f, 1);
			//mercury.material = mercuryMaterial;

			//// Venus
			//SceneObject venusRoot = new SceneObject();
			//venusRoot.AddComponent(new SimpleTransformAnimationComponent()
			//{
			//	RotationSpeed = new Vector3(1.3f * speed, 0, 0),
			//});
			//SceneObject venus = new SceneObject();
			//venus.model = sphere;
			//venus.transform.LocalPositionX = 0.67f * orbitDistanceMultiplier;
			//venus.transform.LocalScale = Vector3.One * 0.95f * planetScaleMultiplier;
			//venus.transform.parent = venusRoot.transform;
			//SimpleMaterial venusMaterial = baseMaterial.CreateInstanceTyped();
			//venusMaterial.pixelShaderData.DiffuseColor = new Vector4(0.96f, 0.48f, 0.04f, 1);
			//venus.material = venusMaterial;

			//// Earth + Moon
			//var earth = CreatePlanet(1f * speed, 0.93f * orbitDistanceMultiplier, 1f * planetScaleMultiplier, baseMaterial, new Vector4(0.2f, 0.2f, 0.8f, 1), sphere);
			//CreatePlanet(1 / 0.1f * speed, 1f * orbitDistanceMultiplier, 0.8f * planetScaleMultiplier, baseMaterial, new Vector4(0.4f, 0.4f, 0.4f, 1), sphere, earth.transform);

			//// Mars
			//CreatePlanet(0.45f * speed, 1.41f * orbitDistanceMultiplier, 0.53f * planetScaleMultiplier, baseMaterial, new Vector4(0.73f, 0.13f, 0.04f, 1), sphere);

			//// Jupiter
			//CreatePlanet(1 / 11.9f * speed, 2.6f * orbitDistanceMultiplier, 11.12f * planetScaleMultiplier, baseMaterial, new Vector4(0.77f, 0.66f, 0.45f, 1), sphere);

			//// Saturn
			//CreatePlanet(1 / 29.5f * speed, 3.7f * orbitDistanceMultiplier, 9.45f * planetScaleMultiplier, baseMaterial, new Vector4(0.68f, 0.54f, 0.36f, 1), sphere);

			//// Uranus
			//CreatePlanet(1 / 84f * speed, 4.4f * orbitDistanceMultiplier, 4.00f * planetScaleMultiplier, baseMaterial, new Vector4(0.55f, 0.83f, 0.97f, 1), sphere);

			//// Neptune
			//CreatePlanet(1 / 165f * speed, 5.2f * orbitDistanceMultiplier, 3.88f * planetScaleMultiplier, baseMaterial, new Vector4(0.38f, 0.50f, 0.83f, 1), sphere);


			// Create camera
			Camera camera = new Camera();
			camera.cameraPosition = new Vector3(0, 3, 10.0f);
			camera.cameraLookAt = new Vector3(0, 0, 0);
			camera.fov = (float)Math.PI / 4.0f;
			camera.aspectRatio = renderer.form.Width / (float)renderer.form.Height;


			//SceneManager.SaveCurrentHierarchyRelative("Test scene 01.json");

			// Use clock
			Time.Start();

			bool isOnFirstScene = true;

			// Main loop
			RenderLoop.Run(renderer.form, () =>
			{
				// Advance time
				Time.NextFrame();

				// Demonstrate switching scenes
				if (isOnFirstScene)
				{
					if ((int)Time.Elapsed % 8 >= 4)
					{
						isOnFirstScene = false;
						SceneManager.LoadAndReplaceHierarchyRelative("Earth and moon.json", renderer);
					}
				}
				else
				{
					if ((int)Time.Elapsed % 8 < 4)
					{
						isOnFirstScene = true;
						SceneManager.LoadAndReplaceHierarchyRelative("Solar system.json", renderer);
					}
				}

				// Clear views
				renderer.ClearDepthStencilView();
				renderer.ClearRenderTargetView();


				// Update objects
				Hierarchy.Update();

				// Render
				Hierarchy.Render(camera, renderer);


				// Present
				renderer.swapChain.Present(0, PresentFlags.None);
			});


			//SceneManager.SaveCurrentHierarchyRelative("Solar system.json");

			//// Release all resources
			renderer.Dispose();
			Hierarchy.Dispose();
		}

		private static SceneObject CreatePlanet(float orbitRotationSpeed, float orbitDistance, float planetScale, SimpleMaterial baseMaterial, Vector4 planetColor, Model model, Transform parentTransform = null)
		{
			SceneObject planetRoot = new SceneObject();
			planetRoot.AddComponent(new SimpleTransformAnimationComponent()
			{
				RotationSpeed = new Vector3(orbitRotationSpeed, 0, 0),
			});
			planetRoot.transform.parent = parentTransform;
			SceneObject planet = new SceneObject();
			planet.model = model;
			planet.transform.LocalPositionX = orbitDistance;
			planet.transform.LocalScale = Vector3.One * planetScale;
			planet.transform.parent = planetRoot.transform;
			SimpleMaterial planetMaterial = baseMaterial.CreateInstanceTyped();
			planetMaterial.pixelShaderData.DiffuseColor = planetColor;
			planet.material = planetMaterial;

			return planet;
		}
	}
}