using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace Spark.Application.Core.WindowNS;
public class Window
{
    private CancellationToken _ct;
    private Sdl2Window _window;
    private bool _windowResized = false;
    public WindowData WindowData { get; set; }

    public Window(WindowData windowData, CancellationToken ct)
    {
        WindowData = windowData;
        _ct = ct;
    }

    ~Window()
    {
        _window.Close();
    }

    public void Initialize()
    {
        var windowCreateInfo = new WindowCreateInfo
        {
            X = 100,
            Y = 100,
            WindowWidth = WindowData.WindowWidth,
            WindowHeight = WindowData.WindowHeight,
            WindowTitle = WindowData.WindowTitle
        };

        _window = VeldridStartup.CreateWindow(ref windowCreateInfo);
        var graphicsDevice = VeldridStartup.CreateGraphicsDevice(_window, GraphicsBackend.Vulkan);

        _window.Resized += () => _windowResized = true;
    }

    public bool Running() => _window.Exists && !_ct.IsCancellationRequested;
}
