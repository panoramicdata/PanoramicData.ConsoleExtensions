using System.Text;

namespace PanoramicData.ConsoleExtensions;

/// <summary>
/// Provides enhanced console functionality
/// </summary>
public static class ConsolePlus
{
	/// <summary>
	/// Reads a password from the console input, masking characters with asterisks
	/// and supporting backspace for corrections
	/// </summary>
	/// <returns>The password string entered by the user</returns>
	public static string ReadPassword()
	{
		var result = new StringBuilder();
		while (true)
		{
			ConsoleKeyInfo key = Console.ReadKey(true);
			switch (key.Key)
			{
				case ConsoleKey.Enter:
					return result.ToString();
				case ConsoleKey.Backspace:
					if (result.Length == 0)
					{
						continue;
					}

					result.Length--;
					Console.Write("\b \b");
					continue;
				default:
					result.Append(key.KeyChar);
					Console.Write("*");
					continue;
			}
		}
	}
}
