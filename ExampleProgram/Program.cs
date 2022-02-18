using JYCEngine;

// Testing the RenderBuffer class for smooth rendering

Engine.Init();

List<(int x, int y, int r, int vx, int vy)> circles = new();
Random rand = new();
for (int i = 0; i < 400; i++)
{
	int r = (int)(2 + Math.Pow(rand.NextDouble(), 2) * 10);
	circles.Add((rand.Next(r, Engine.ScreenWidth - r), rand.Next(r, Engine.ScreenHeight - r), r, rand.Next(-6, 6), rand.Next(-3, 3)));
}
circles = circles.OrderBy(x => x.r).ToList();
while (true)
{
	for(int i = 0; i < circles.Count; i++)
    {
		(int x, int y, int r, int vx, int vy) = circles[i];
		if (x - 2*r + vx < 0 || x + 2*r + vx >= Engine.ScreenWidth) 
			vx = -vx;
		if (y - r + vy < 0 || y + r + vy >= Engine.ScreenHeight)
			vy = -vy;
		circles[i] = (x + vx, y + vy, r, vx, vy);
    }

	Engine.RenderBuffer.ClearBuffer();

	foreach ((int x, int y, int r, int vx, int vy) in circles)
		DrawCircle(x, y, r);

	Engine.RenderBuffer.Blit();

	Thread.Sleep(33); // About 30 fps
}

static void DrawCircle(int x, int y, int r)
{
	for (int _x = -2*r; _x <= 2*r; _x++)
    {
		for (int _y = -r; _y <= r; _y++)
		{
			if ((int)((_x / 2f) * (_x / 2f)) + _y * _y <= r * r)
			{
				if (-_y >= (r * r) / ((_x / 2f) + r * 1.0f) - r * 1.0f)
					Engine.RenderBuffer.SetCharacter(_x + x, _y + y, '#');
				else if (-_y >= (r * r) / ((_x / 2f) + r * 1.3f) - r * 1.3f)
					Engine.RenderBuffer.SetCharacter(_x + x, _y + y, '-');
				else
					Engine.RenderBuffer.SetCharacter(_x + x, _y + y, ' ');
			}
		}
    }
}