using System;
using JYCEngine;

namespace JYCEngine.StdLib;

public class MotionComponent : Component
{
	public double X { get; set; } = 0;
	public double Y { get; set; } = 0;
	public double Z { get; set; } = 0;

	internal double PrevX { get; private set; } = 0;
	internal double PrevY { get; private set; } = 0;
	internal bool Moved { get; private set; } = true;

	public bool Fixed { get; set; } = false;
	public double VelocityX { get; set; } = 0;
	public double VelocityY { get; set; } = 0;

	public override void Update()
	{
		if (!Fixed)
		{
			if(VelocityX + VelocityY > 0) Moved = true;
			else Moved = false;

			PrevX = X;
			X += VelocityX;

			PrevY = Y;
			Y += VelocityY;
		} else Moved = false;
	}
}