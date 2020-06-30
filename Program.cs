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
			// Create Renderer
			Renderer renderer = new Renderer("SharpDX - SceneManager", 1280, 720, 60);


			// Load model
			//String fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets\\Planets\\sphere.obj");
			ModelLoader modelLoader = new ModelLoader(renderer.device);
			Model sphere = modelLoader.LoadRelative("Assets\\Planets\\sphere.obj");

			// Create scene objects
			SceneObject sunObject = new SceneObject();
			sunObject.model = sphere;

			SceneObject planet01Root = new SceneObject();
			planet01Root.AddComponent(new SimpleTransformAnimationComponent()
			{
				rotationSpeed = new Vector3(0.5f, 0, 0),
			});

			SceneObject planet01 = new SceneObject();
			planet01.model = sphere;
			planet01.transform.LocalPositionX = 2f;
			planet01.transform.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
			planet01.transform.parent = planet01Root.transform;

			SceneObject moon01Root = new SceneObject();
			moon01Root.transform.parent = planet01.transform;
			moon01Root.AddComponent(new SimpleTransformAnimationComponent()
			{
				rotationSpeed = new Vector3(2f, 0, 0),
			});

			SceneObject moon01 = new SceneObject();
			moon01.model = sphere;
			moon01.transform.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
			moon01.transform.LocalPositionX = 1f;
			moon01.transform.parent = moon01Root.transform;


			// Create and setup material
			SimpleMaterial baseMaterial = new SimpleMaterial
			{
				vertexShaderPath = "Shaders.fx",
				vertexShaderEntryPoint = "VS",
				vertexShaderProfile = "vs_5_0",
				pixelShaderPath = "Shaders.fx",
				pixelShaderEntryPoint = "PS",
				pixelShaderProfile = "ps_5_0",
			};
			baseMaterial.Initialize(renderer.device);
			baseMaterial.pixelShaderData.lightPos = new Vector4(0, 0, 0, 1);
			baseMaterial.pixelShaderData.emissionColor = new Vector4(0.1f, 0.1f, 0.1f, 1);


			// Set material options
			SimpleMaterial sunMaterial = baseMaterial.CreateInstanceTyped();
			sunMaterial.pixelShaderData.diffuseColor = new Vector4(0.92f, 0.72f, 0.2f, 1);
			sunMaterial.pixelShaderData.emissionColor = new Vector4(0.92f, 0.8f, 0.2f, 1);
			sunObject.material = sunMaterial;

			SimpleMaterial planet01Material = baseMaterial.CreateInstanceTyped();
			planet01Material.pixelShaderData.diffuseColor = new Vector4(0.2f, 0.8f, 0.2f, 1);
			planet01.material = planet01Material;

			SimpleMaterial moon01Material = baseMaterial.CreateInstanceTyped();
			moon01Material.pixelShaderData.diffuseColor = new Vector4(0.4f, 0.4f, 0.4f, 1);
			moon01.material = moon01Material;


			// Create camera
			Camera camera = new Camera();
			camera.cameraPosition = new Vector3(0, 3, 5.0f);
			camera.cameraLookAt = new Vector3(0, 0, 0);
			camera.fov = (float)Math.PI / 4.0f;
			camera.aspectRatio = renderer.form.Width / (float)renderer.form.Height;


			// Use clock
			Time.Start();

			// Main loop
			RenderLoop.Run(renderer.form, () =>
			{
				// Advance time
				Time.NextFrame();

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


			// Release all resources
			baseMaterial.Dispose();
			sunObject.Dispose();
			planet01Root.Dispose();
			planet01.Dispose();
			moon01Root.Dispose();
			moon01.Dispose();
			renderer.Dispose();
		}
	}
}