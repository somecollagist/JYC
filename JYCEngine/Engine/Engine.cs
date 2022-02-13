using System;
using System.Collections.Generic;
using System.Linq;

namespace JYCEngine;
public static partial class Engine
{
	/// <summary>
	/// Stores all the components used by the game. Sometimes referred to as the master component registry.
	/// </summary>
	public static Dictionary<string, Component> Components { get; internal set; } = new();

	/// <summary>
	/// Width of a row of text at the current resolution.
	/// </summary>
	public static int ScreenWidth { get; private set; } = 0;
	/// <summary>
	/// How many rows of text can be displayed at the current resolution.
	/// </summary>
	public static int ScreenHeight { get; private set; } = 0;

	/// <summary>
	/// Runs setup for the engine.
	/// </summary>
	/// <param name="info">The game's metadata, which can be displayed.</param>
	public static void Init(GameInfo? info = null)
	{
		if (info != null) Console.Write(info.GenerateOutput());
		Console.WriteLine("Please resize this window to something you're happy with, then press any key to continue...");
		Console.ReadKey();

		ScreenHeight = Console.BufferHeight;
		ScreenWidth = Console.BufferWidth;
	}
}