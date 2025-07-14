using Microsoft.Extensions.Logging;

namespace PanoramicData.ConsoleExtensions;

/// <summary>
/// Configuration options for the ConsoleLogger
/// </summary>
public class ConsoleLoggerOptions
{
	/// <summary>
	/// Initializes a new instance of ConsoleLoggerOptions with default values
	/// </summary>
	public ConsoleLoggerOptions()
	{
	}

	/// <summary>
	/// Initializes a new instance of ConsoleLoggerOptions with a specified minimum log level
	/// </summary>
	/// <param name="logLevel">The minimum log level to display</param>
	public ConsoleLoggerOptions(LogLevel logLevel)
	{
		LogLevel = logLevel;
	}

	/// <summary>
	/// Gets or sets the console color for trace level logs. Default is White.
	/// </summary>
	public ConsoleColor TraceColor { get; set; } = ConsoleColor.White;
	
	/// <summary>
	/// Gets or sets the console color for debug level logs. Default is Cyan.
	/// </summary>
	public ConsoleColor DebugColor { get; set; } = ConsoleColor.Cyan;
	
	/// <summary>
	/// Gets or sets the console color for information level logs. Default is Gray.
	/// </summary>
	public ConsoleColor InformationColor { get; set; } = ConsoleColor.Gray;
	
	/// <summary>
	/// Gets or sets the console color for warning level logs. Default is Yellow.
	/// </summary>
	public ConsoleColor WarningColor { get; set; } = ConsoleColor.Yellow;
	
	/// <summary>
	/// Gets or sets the console color for error level logs. Default is Red.
	/// </summary>
	public ConsoleColor ErrorColor { get; set; } = ConsoleColor.Red;
	
	/// <summary>
	/// Gets or sets the console color for critical level logs. Default is Magenta.
	/// </summary>
	public ConsoleColor CriticalColor { get; set; } = ConsoleColor.Magenta;
	
	/// <summary>
	/// Gets or sets the console color for none level logs. Default is DarkGray.
	/// </summary>
	public ConsoleColor NoneColor { get; set; } = ConsoleColor.DarkGray;
	
	/// <summary>
	/// Gets or sets the minimum log level. Default is Information.
	/// </summary>
	public LogLevel LogLevel { get; set; } = LogLevel.Information;
	
	/// <summary>
	/// Gets or sets the time format string for timestamps. Default is "yyyy-MM-dd HH:mm:ss.fff".
	/// </summary>
	public string TimeFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

	/// <summary>
	/// Gets or sets whether to include timestamps in log output. Default is false.
	/// </summary>
	public bool IncludeTimestamp { get; set; } = false;

	/// <summary>
	/// Gets or sets whether to include log level in output. Default is true.
	/// </summary>
	public bool IncludeLogLevel { get; set; } = true;

	/// <summary>
	/// Gets or sets whether to include event ID in output. Default is false.
	/// </summary>
	public bool IncludeEventId { get; set; } = false;

	/// <summary>
	/// Gets or sets whether to enable structured logging formatting. Default is true.
	/// </summary>
	public bool EnableStructuredLogging { get; set; } = true;

	/// <summary>
	/// Gets or sets whether to enable clickable hyperlinks. Default is true.
	/// </summary>
	public bool EnableHyperlinks { get; set; } = true;

	/// <summary>
	/// Gets or sets the format for structured log output. Default is "{Timestamp} [{Level}] {Message}".
	/// Available placeholders: {Timestamp}, {Level}, {EventId}, {Message}, {Exception}
	/// </summary>
	public string OutputFormat { get; set; } = "{Timestamp} [{Level}] {Message}";

	/// <summary>
	/// Gets or sets whether to use single line format for exceptions. Default is false.
	/// </summary>
	public bool SingleLineExceptions { get; set; } = false;
}