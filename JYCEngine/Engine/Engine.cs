using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace JYCEngine;
public static partial class Engine
{
	/// <summary>
	/// Stores all the components used by the game. Sometimes referred to as the master component registry.
	/// </summary>
	public static Dictionary<string, Component> Components { get; internal set; } = new();

	/// <summary>
	/// Stores all the entities used by the game.
	/// </summary>
	public static List<Entity> Entities { get; internal set; } = new();

	/// <summary>
	/// Stores all the registered systems used by the game.
	/// </summary>
	public static List<ISystem> Systems { get; internal set; } = new();

	/// <summary>
	/// Width of a row of text at the current resolution.
	/// </summary>
	public static int ScreenWidth { get; private set; } = 0;
	/// <summary>
	/// How many rows of text can be displayed at the current resolution.
	/// </summary>
	public static int ScreenHeight { get; private set; } = 0;

	/// <summary>
	/// Render Buffer for low level drawing to the console
	/// </summary>
	public static RenderBuffer RenderBuffer { get; private set; }

	/// <summary>
	/// Runs setup for the engine.
	/// </summary>
	/// <param name="info">The game's metadata, which can be displayed.</param>
	public static async void Init(GameInfo? info = null)
	{
		if (info != null) Console.Write(info.GenerateOutput());
		Console.WriteLine("Please resize this window to something you're happy with, then press any key to continue...");
		Console.ReadKey();

		ScreenHeight = Console.WindowHeight;
		ScreenWidth = Console.WindowWidth;

		RenderBuffer = new RenderBuffer(ScreenWidth, ScreenHeight);

		Systems.Add(new StdLib.RenderingSystem());

		while (true)
        {
			await Tick();
        }
		/*
		Renderer.Init(24);

		while(true)
		{
			// Await cycle to prevent drawing frames concurrently.
			await Renderer.Cycle();
			foreach(Component component in Components.Values)
			{
				component.Update();
			}
		}
		*/
	}

	private static Task Tick()
    {
		Systems.ForEach(x => x.Execute(0f)); // Replace 0f with time since last frame
		System.Threading.Thread.Sleep(1000 / 30); // 30 FPS
		return Task.CompletedTask;
	}
}