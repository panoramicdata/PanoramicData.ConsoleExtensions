using System.Text;

namespace PanoramicData.ConsoleExtensions
{
	public static class ConsolePlus
	{
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
}
