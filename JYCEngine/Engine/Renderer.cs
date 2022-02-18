using System;
using System.ComponentModel;
using JYCEngine.StdLib;

namespace JYCEngine;
public static partial class Engine
{
	internal static class Renderer
	{
		/// <summary>
		/// Width of a row of text at the current resolution.
		/// </summary>
		public static int ScreenWidth { get; private set; } = 0;
		/// <summary>
		/// How many rows of text can be displayed at the current resolution.
		/// </summary>
		public static int ScreenHeight { get; private set; } = 0;

		/// <summary>
		/// The framerate the renderer should run at.
		/// </summary>
		public static int Framerate {get; private set;} = 0;

		/// <summary>
		/// The image which will be shown to the user.
		/// </summary>
		private static char[,] Buffer { get; set; } = new char[ScreenWidth, ScreenHeight];

		/// <summary>
		/// Sets up the renderer.
		/// </summary>
		/// <param name="framerate">The framerate for the renderer.</param>
		public static void Init(int framerate)
		{
			ScreenHeight = Console.BufferHeight;
			ScreenWidth = Console.BufferWidth;
			Framerate = framerate;

			Buffer = new char[ScreenWidth, ScreenHeight];
			// This is how Jon Skeet did it, do you dare question him?
			for(int y = 0; y < Buffer.GetLength(1); y++)
			{
				for(int x = 0; x < Buffer.GetLength(0); x++)
				{
					Buffer[x,y] = ' ';
				}
			}
		}

		/// <summary>
		/// Wrapper to draw the frame, print to output, and wait for the next frame.
		/// </summary>
		/// <returns></returns>
		internal static Task Cycle()
		{
			DrawFrame();
			PrintBuffer();
			System.Threading.Thread.Sleep(1000/Framerate);
			return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		private static void DrawFrame()
		{
			// ImageComponent[] imgcs = Engine
			// 							.GetComponentsByType<ImageComponent>()
			// 							.OrderBy(t => t.Z)
			// 							.ToArray();

			// foreach (ImageComponent imgc in imgcs)
			// {
			// 	for(int y = 0; y < imgc.Image.GetLength(1); y++)
			// 	{
			// 		for(int x = 0; x < imgc.Image.GetLength(0); x++)
			// 		{
			// 			try
			// 			{
			// 				Buffer[x + (int)imgc.PrevX, y + (int)imgc.PrevY] = ' ';
			// 			}
			// 			// Throws if the object is rendered out of frame.
			// 			catch(IndexOutOfRangeException) {}
			// 		}
			// 	}

			// 	for (int y = 0; y < imgc.Image.GetLength(1); y++)
			// 	{
			// 		for (int x = 0; x < imgc.Image.GetLength(0); x++)
			// 		{
			// 			try
			// 			{
			// 				Buffer[x + (int)imgc.X, y + (int)imgc.Y] = imgc.Image[x, y];
			// 			}
			// 			// Throws if the object is rendered out of frame.
			// 			catch(IndexOutOfRangeException) {}
			// 		}
			// 	}
			// }
		}

		public static void PrintBuffer()
		{
			string frame = string.Join('\n', Enumerable
							.Range(0, Buffer.GetLength(1))
							.Select(y => new string(Enumerable
											.Range(0, Buffer.GetLength(0))
											.Select(x => Buffer[x, y])
											.ToArray())
											));
			Console.Clear();
			Console.Write(frame);
		}
	}
}