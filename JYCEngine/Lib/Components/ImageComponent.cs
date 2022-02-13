using System;
using System.Collections.Generic;

using JYCEngine;

namespace JYCEngine.StdLib;

public class ImageComponent : Component
{
	public char[,] Image { get; private set; } = new char[0, 0];

	public void LoadImageFromCharArray(char[,] img)
	{
		Array.Copy(img, Image, img.Length);
	}

	public void LoadImageFromString(string img, int width, int height)
	{
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Image[x, y] = img[(y * width) + x];
			}
		}
	}
}