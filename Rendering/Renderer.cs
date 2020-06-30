using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Device = SharpDX.Direct3D11.Device;

namespace SceneManager
{
	public class Renderer
	{
		public Device device;
		public DeviceContext context;
		public Control form;

		public DepthStencilView depthView;
		public RenderTargetView renderView;

		public SwapChainDescription swapChainDescription;
		public SwapChain swapChain;

		public Texture2D backBuffer;
		public Factory factory;

		#region Initialization
		/// <summary>
		/// Initializes all the DirectX boilerplate required to render models.
		/// </summary>
		/// <param name="windowName">The name of the application window.</param>
		/// <param name="windowWidth">Window width in pixels.</param>
		/// <param name="windowHeight">Window height in pixels.</param>
		/// <param name="fps">Target frames per second.</param>
		public Renderer(string windowName, int windowWidth, int windowHeight, int fps)
		{
			form = new RenderForm(windowName);
			form.Width = windowWidth;
			form.Height = windowHeight;

			swapChainDescription = new SwapChainDescription()
			{
				BufferCount = 1,
				ModeDescription =
					new ModeDescription(form.Width, form.Height,
										new Rational(fps, 1), Format.R8G8B8A8_UNorm),
				IsWindowed = true,
				OutputHandle = form.Handle,
				SampleDescription = new SampleDescription(1, 0),
				SwapEffect = SwapEffect.Discard,
				Usage = Usage.RenderTargetOutput
			};
			Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug, swapChainDescription, out device, out swapChain);

			context = device.ImmediateContext;

			// Ignore all windows events
			factory = swapChain.GetParent<Factory>();
			factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

			// Create render view and depth buffer
			backBuffer = Texture2D.FromSwapChain<Texture2D>(swapChain, 0);
			renderView = new RenderTargetView(device, backBuffer);

			var depthBuffer = new Texture2D(device, new Texture2DDescription()
			{
				Format = Format.D32_Float_S8X24_UInt,
				ArraySize = 1,
				MipLevels = 1,
				Width = form.Width,
				Height = form.Height,
				SampleDescription = new SampleDescription(1, 0),
				Usage = ResourceUsage.Default,
				BindFlags = BindFlags.DepthStencil,
				CpuAccessFlags = CpuAccessFlags.None,
				OptionFlags = ResourceOptionFlags.None
			});
			depthView = new DepthStencilView(device, depthBuffer);


			Init();
		}

		public void Init()
		{
			context.Rasterizer.SetViewports(new Viewport(0, 0, form.Width, form.Height, 0.0f, 1.0f));
			context.OutputMerger.SetTargets(depthView, renderView);
		}
		#endregion


		#region Rendering
		public void ClearDepthStencilView()
		{
			context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
		}
		public void ClearRenderTargetView()
		{
			context.ClearRenderTargetView(renderView, Color.Black);
		}
		#endregion


		#region Clean up
		public void Dispose()
		{
			renderView.Dispose();
			context.ClearState();
			context.Flush();
			context.Dispose();
			device.Dispose();
			backBuffer.Dispose();
			swapChain.Dispose();
			factory.Dispose();
		}
		#endregion
	}
}
