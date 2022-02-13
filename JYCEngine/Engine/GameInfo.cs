namespace JYCEngine;

/// <summary>
/// Metadata of a game.
/// </summary>
public class GameInfo
{
	/// <summary>
	/// The name of the game.
	/// </summary>
	public string? Name { get; set; }
	/// <summary>
	/// A description of the game.
	/// </summary>
	public string? Description { get; set; }
	/// <summary>
	/// The author of the game.
	/// </summary>
	public string? Author { get; set; }

	/// <summary>
	/// Creates a string to represent the game's metadata.
	/// </summary>
	/// <returns>A string representing the game's metadata.</returns>
	public string GenerateOutput()
	{
		string ret = "";
		if (Name != null) ret += $"-=[ {Name} ]=-\n";
		if (Description != null) ret += $"\t {Description}\n";
		if (Author != null) ret += "Developed by {Author}\n";

		return ret;
	}
}