using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JYCEngine;

/// <summary>
/// Provides an abstraction layer to access the Console's raw output stream
/// via a buffer
/// </summary>
public class RenderBuffer
{
    /// <summary>
    /// Width of a row of text at the current resolution.
    /// </summary>
    public readonly int Width;
    /// <summary>
    /// How many rows of text can be displayed at the current resolution.
    /// </summary>
    public readonly int Height;

    /// <summary>
    /// Buffer array stored as 1D byte array for performance
    /// </summary>
    private byte[] buffer;

    /// <summary>
    /// Raw output stream for the console
    /// </summary>
    private Stream outputStream;

    public RenderBuffer(int width, int height)
    {
        Width = width / 2;
        Height = height;
        buffer = new byte[Width * Height * 2];
        ClearBuffer();
        outputStream = Console.OpenStandardOutput();
        Console.CursorVisible = false;
    }

    /// <summary>
    /// Set an individual character at a position on the screen
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="c"></param>
    public void SetCharacter(int x, int y, char c)
    {
        if (!InBounds(x, Height - 1 - y)) return;
        int i = (Height - 1 - y) * (Width * 2) + x * 2;
        buffer[i] = (byte)c;
    }

    /// <summary>
    /// Set a 2d chunk of characters at a position on the screen
    /// </summary>
    /// <param name="x">X Position</param>
    /// <param name="y">Y Position</param>
    /// <param name="chars">2D array of characters to set</param>
    /// <param name="includeSpace">Boolean flag for whether to override with space characters</param>
    public void SetCharacters(int x, int y, char[,] chars, bool includeSpace = false)
    {
        if (chars == null) return;

        for (int fy = 0; fy < chars.GetLength(1); fy++)
        {
            for (int fx = 0; fx < chars.GetLength(0); fx++)
            {
                if (InBounds(x + fx, Height - 1 - (y + fy)) && (includeSpace || (chars[fx, fy] != ' ' && chars[fx, fy] != '\0')))
                {
                    int i = (Height - 1 - (y + fy)) * (Width * 2) + (x + fx) * 2;
                    buffer[i] = (byte)chars[fx, fy];
                }
            }
        }
    }

    public void PrintString(int x, int y, string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            SetCharacter(x + i, y, str[i]);
        }
    }

    /// <summary>
    /// Check whether a given pixel is inside the console bounds
    /// </summary>
    /// <param name="x">X Position</param>
    /// <param name="y">Y Position</param>
    /// <returns></returns>
    public bool InBounds(int x, int y) => 0 <= x && x < Width && 0 <= y && y < Height;

    /// <summary>
    /// Push the buffer to the screen
    /// </summary>
    public void Blit()
    {
        Console.SetCursorPosition(0, 0);
        outputStream.Write(buffer, 0, Width * Height * 2);
        outputStream.Flush();
    }

    /// <summary>
    /// Clear the buffer
    /// </summary>
    public void ClearBuffer()
    {
        Fill(' ');
    }

    /// <summary>
    /// Fill the buffer with a character
    /// </summary>
    /// <param name="c">Character to fill with</param>
    public void Fill(char c)
    {
        for (int i = 0; i < buffer.Length; i++) buffer[i] = (byte)c;
    }
}