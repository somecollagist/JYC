using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine;

public static class Engine
{
    public static RenderBuffer RenderBuffer { get; private set; }
    public static Action Tick { get; set; }
    public static int TargetFramerate { get; set; } = 30;
    public static float TimeScale { get; set; } = 1f;

    private static float _deltaTime;
    public static float DeltaTime => _deltaTime * TimeScale;

    public static void Init()
    {
        Console.WriteLine("Resize the window and press any key!");
        Console.ReadKey();

        RenderBuffer = new RenderBuffer(Console.WindowWidth, Console.WindowHeight);
    }

    public static void Run()
    {
        _deltaTime = 0;
        while (true)
        {
            long time = Stopwatch.GetTimestamp();

            Input.Run();
            Tick?.Invoke();

            float frametime = (float)(Stopwatch.GetTimestamp() - time) / Stopwatch.Frequency;
            int waitTime = (int)((1f / TargetFramerate - frametime) * 1000);
            if (waitTime > 0)
                Thread.Sleep(waitTime);

            _deltaTime = (float)(Stopwatch.GetTimestamp() - time) / Stopwatch.Frequency;

#if DEBUG
            string framerateString = ((int)(1f / DeltaTime)).ToString();
            for (int i = 0; i < framerateString.Length; i++)
                RenderBuffer.SetCharacter(RenderBuffer.Width - framerateString.Length + i, RenderBuffer.Height - 1, framerateString[i]);
# endif
        }
    }
}
