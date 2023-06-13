using Microsoft.Extensions.Logging;

namespace PanoramicData.ConsoleExtensions
{
	public class ConsoleLoggerOptions
{
	public ConsoleColor TraceColor {get;} = ConsoleColor.White;
	public ConsoleColor DebugColor {get;} = ConsoleColor.Cyan;
	public ConsoleColor InformationColor {get;} = ConsoleColor.Gray;
	public ConsoleColor WarningColor {get;} = ConsoleColor.Yellow;
	public ConsoleColor ErrorColor {get;} = ConsoleColor.Red;
	public ConsoleColor CriticalColor {get;} = ConsoleColor.Magenta;
	public ConsoleColor NoneColor {get;} = ConsoleColor.DarkGray;
	public LogLevel LogLevel {get;} = LogLevel.Information;
	public string TimeFormat {get;} = "yyyy-MM-dd HH:mm:ss.fff";
}

}