using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine.Engine
{
    /// <summary>
    /// Class to store data to be shown on the console window
    /// </summary>
    public class RenderBuffer
    {
        public readonly int Width;
        public readonly int Height;

        public readonly int BufferCount;

        private char[] buffer;
        private Stream outputStream;

        public RenderBuffer(int width, int height, int bufferCount = 2)
        {
            Width = width;
            Height = height;
            BufferCount = bufferCount;

            buffer = new char[height * width];

            outputStream = Console.OpenStandardOutput();
        }

        public void Blit()
        {
            outputStream.Seek(0, SeekOrigin.Begin);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    outputStream.Write()
                }
            }
        }
    }
}
