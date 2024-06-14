using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spark.Util;

public static class Logger
{
    public static void Trace(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;   
        Console.WriteLine($"[TRACE] {message}");
    }

    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[INFO] {message}");
    }

    public static void Warn(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARN] {message}");
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
    }

    public static void Fatal(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"[FATAL] {message}");
    }

    public static void ResetColor()
    {
        Console.ResetColor();
    }
}
