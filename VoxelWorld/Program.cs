using Spark.Util;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.SPIRV;
using Veldrid.StartupUtilities;

class Program
{
    struct VertexPositionColor
    {
        public Vector2 Position;
        public RgbaFloat Color;

        public VertexPositionColor(Vector2 position, RgbaFloat color)
        {
            Position = position;
            Color = color;
        }
    }

    static void Main()
    {
        WindowCreateInfo windowCI = new WindowCreateInfo
        {
            X = 100,
            Y = 100,
            WindowWidth = 800,
            WindowHeight = 600,
            WindowTitle = "Veldrid Window"
        };
        Sdl2Window window = VeldridStartup.CreateWindow(ref windowCI);

        GraphicsDeviceOptions options = new GraphicsDeviceOptions
        {
            PreferStandardClipSpaceYDirection = true,
            PreferDepthRangeZeroToOne = true,
            Debug = false
        };

        GraphicsDevice gd = VeldridStartup.CreateGraphicsDevice(window, options, GraphicsBackend.Vulkan);

        VertexPositionColor[] vertices =
        {
            new VertexPositionColor(new Vector2(0f, 0.5f), RgbaFloat.Red),
            new VertexPositionColor(new Vector2(0.5f, -0.5f), RgbaFloat.Green),
            new VertexPositionColor(new Vector2(-0.5f, -0.5f), RgbaFloat.Blue)
        };

        DeviceBuffer vertexBuffer = gd.ResourceFactory.CreateBuffer(new BufferDescription(
            (uint)(vertices.Length * sizeof(float) * 6), BufferUsage.VertexBuffer));

        gd.UpdateBuffer(vertexBuffer, 0, vertices);

        string vertexShaderCode = @"
        #version 450
        layout(location = 0) in vec2 Position;
        layout(location = 1) in vec4 Color;
        layout(location = 0) out vec4 fsin_Color;
        void main()
        {
            gl_Position = vec4(Position, 0, 1);
            fsin_Color = Color;
        }";

        string fragmentShaderCode = @"
        #version 450
        layout(location = 0) in vec4 fsin_Color;
        layout(location = 0) out vec4 fsout_Color;
        void main()
        {
            fsout_Color = fsin_Color;
        }";


        Shader[] shaders = gd.ResourceFactory.CreateFromSpirv(
            new ShaderDescription(ShaderStages.Vertex, Encoding.ASCII.GetBytes(vertexShaderCode), "main"),
            new ShaderDescription(ShaderStages.Fragment, Encoding.ASCII.GetBytes(fragmentShaderCode), "main"));

        VertexLayoutDescription vertexLayout = new VertexLayoutDescription(
            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));

        // Create the resource layouts (empty for this simple example)
        ResourceLayout[] resourceLayouts = Array.Empty<ResourceLayout>();

        // Create the graphics pipeline
        GraphicsPipelineDescription pipelineDescription = new GraphicsPipelineDescription
        {
            BlendState = BlendStateDescription.SingleOverrideBlend,
            DepthStencilState = DepthStencilStateDescription.Disabled,
            RasterizerState = RasterizerStateDescription.Default,
            PrimitiveTopology = PrimitiveTopology.TriangleList,
            ShaderSet = new ShaderSetDescription(
                vertexLayouts: new[] { vertexLayout },
                shaders: shaders),
            Outputs = gd.MainSwapchain.Framebuffer.OutputDescription,
            ResourceLayouts = resourceLayouts
        };

        Pipeline pipeline = gd.ResourceFactory.CreateGraphicsPipeline(ref pipelineDescription);

        CommandList commandList = gd.ResourceFactory.CreateCommandList();

        var sw = new Stopwatch();
        sw.Start();
        long previousTicks = sw.ElapsedTicks;

        while (window.Exists)
        {

            long currentTicks = sw.ElapsedTicks;
            long elapsedTicks = currentTicks - previousTicks;
            double ns = 1000000000.0 * elapsedTicks / Stopwatch.Frequency;
            double frameTime = 1 / (ns / 1000000000.0);

            Logger.Info($"FPS: {frameTime:F2} FPS");


            Sdl2Events.ProcessEvents();

            commandList.Begin();
            commandList.SetFramebuffer(gd.MainSwapchain.Framebuffer);
            commandList.ClearColorTarget(0, RgbaFloat.Black);
            commandList.SetVertexBuffer(0, vertexBuffer);
            commandList.SetPipeline(pipeline);
            commandList.Draw((uint)vertices.Length);
            commandList.End();
             
            gd.SubmitCommands(commandList);
            gd.SwapBuffers(gd.MainSwapchain);
            gd.WaitForIdle();

            previousTicks = currentTicks;
        }

        gd.Dispose();
    }
}

