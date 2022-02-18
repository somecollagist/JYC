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
<<<<<<< HEAD
=======
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
<<<<<<< Updated upstream
=======
>>>>>>> e2b9cc5bbc83a5c459a502fd8b5fb7533cdb4db4
>>>>>>> Stashed changes
	/// Runs setup for the engine.
	/// </summary>
	/// <param name="info">The game's metadata, which can be displayed.</param>
	public static async void Init(GameInfo? info = null)
	{
		if (info != null) Console.Write(info.GenerateOutput());
		Console.WriteLine("Please resize this window to something you're happy with, then press any key to continue...");
		Console.ReadKey();

<<<<<<< Updated upstream
=======
<<<<<<< HEAD
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
=======
>>>>>>> Stashed changes
		ScreenHeight = Console.WindowHeight;
		ScreenWidth = Console.WindowWidth;

		RenderBuffer = new RenderBuffer(ScreenWidth, ScreenHeight);
<<<<<<< Updated upstream
=======
>>>>>>> e2b9cc5bbc83a5c459a502fd8b5fb7533cdb4db4
>>>>>>> Stashed changes
	}
}