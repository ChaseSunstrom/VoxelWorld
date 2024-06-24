using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Application.Core.WindowNS;
public struct WindowData
{
    public int WindowWidth { get; set; }
    public int WindowHeight { get; set; }
    public string WindowTitle { get; set; }

    public WindowData(int windowWidth, int windowHeight, string windowTitle)
    {
        WindowWidth = windowWidth;
        WindowHeight = windowHeight;
        WindowTitle = windowTitle;
    }
}
