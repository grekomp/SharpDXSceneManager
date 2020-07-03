using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Device = SharpDX.Direct3D11.Device;

namespace SceneManager
{
	/// <summary>
	/// Handles all the DirectX resources necessary for rendering.
	/// 
	/// For more info about any of the DirectX specific classes, see the DirectX documentation.
	/// </summary>
	public class Renderer
	{
		/// <summary>
		/// DirectX device used by this renderer.
		/// </summary>
		public Device device;
		/// <summary>
		/// DirectX device context used by this renderer.
		/// </summary>
		public DeviceContext context;
		/// <summary>
		/// The windows form that this renderer renders to.
		/// </summary>
		public Control form;

		/// <summary>
		/// The depth stencil view used for depth testing.
		/// </summary>
		public DepthStencilView depthView;
		/// <summary>
		/// The target DirectX render view.
		/// </summary>
		public RenderTargetView renderView;

		/// <summary>
		/// Swap chain description used by this renderer's swap chain.
		/// </summary>
		public SwapChainDescription swapChainDescription;
		/// <summary>
		/// The DirectX swap chain used by this renderer.
		/// </summary>
		public SwapChain swapChain;

		/// <summary>
		/// The texture used as a backBuffer for rendering.
		/// </summary>
		public Texture2D backBuffer;
		/// <summary>
		/// The DXGI factory used by this renderer.
		/// </summary>
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

			var viewport = new RawViewportF();
			viewport.Width = form.Width;
			viewport.Height = form.Height;
			viewport.X = 0;
			viewport.Y = 0;
			viewport.MinDepth = 0f;
			viewport.MaxDepth = 1f;


			context.Rasterizer.SetViewport(viewport);
			context.OutputMerger.SetTargets(depthView, renderView);
		}
		#endregion


		#region Rendering
		/// <summary>
		/// Clears the render view.
		/// </summary>
		public void ClearViews()
		{
			ClearDepthStencilView();
			ClearRenderTargetView();
		}

		private void ClearDepthStencilView()
		{
			context.ClearDepthStencilView(depthView, DepthStencilClearFlags.Depth, 1.0f, 0);
		}
		private void ClearRenderTargetView()
		{
			context.ClearRenderTargetView(renderView, Color.Black);
		}
		#endregion


		#region Clean up
		/// <summary>
		/// Releases all managed resources.
		/// </summary>
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
