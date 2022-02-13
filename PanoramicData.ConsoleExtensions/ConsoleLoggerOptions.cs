using Microsoft.Extensions.Logging;

namespace PanoramicData.ConsoleExtensions
{
	public class ConsoleLoggerOptions
	{
		public ConsoleColor TraceColor = ConsoleColor.White;
		public ConsoleColor DebugColor = ConsoleColor.Cyan;
		public ConsoleColor InformationColor = ConsoleColor.Gray;
		public ConsoleColor WarningColor = ConsoleColor.Yellow;
		public ConsoleColor ErrorColor = ConsoleColor.Red;
		public ConsoleColor CriticalColor = ConsoleColor.Magenta;
		public ConsoleColor NoneColor = ConsoleColor.DarkGray;
		public LogLevel LogLevel = LogLevel.Information;
		public string? TimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
	}
}