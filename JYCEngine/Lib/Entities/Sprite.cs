using System;
using JYCEngine;

namespace JYCEngine.StdLib;

public class Sprite : Entity
{
	public Sprite()
	{
		RegisterNewComponent("image", new ImageComponent());
	}
}