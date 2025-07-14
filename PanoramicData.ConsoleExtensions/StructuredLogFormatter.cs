using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace PanoramicData.ConsoleExtensions;

/// <summary>
/// Provides structured formatting for console log messages
/// </summary>
internal static class StructuredLogFormatter
{
	private static readonly Regex UrlRegex = new(@"(https?://[^\s]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
	private static readonly Regex FilePathRegex = new(@"([a-zA-Z]:\\(?:[^\\/:*?""<>|\r\n]+\\)*[^\\/:*?""<>|\r\n]*)", RegexOptions.Compiled);

	/// <summary>
	/// Formats a log message with structured logging support
	/// </summary>
	public static string FormatMessage<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter,
		ConsoleLoggerOptions options)
	{
		var message = formatter(state, exception);
		
		if (!options.EnableStructuredLogging)
		{
			return message;
		}

		var sb = new StringBuilder();
		var format = options.OutputFormat;

		// Replace placeholders
		if (options.IncludeTimestamp && format.Contains("{Timestamp}"))
		{
			var timestamp = DateTime.Now.ToString(options.TimeFormat, CultureInfo.InvariantCulture);
			format = format.Replace("{Timestamp}", timestamp);
		}
		else
		{
			format = format.Replace("{Timestamp}", string.Empty).Trim();
		}

		if (options.IncludeLogLevel && format.Contains("{Level}"))
		{
			var levelString = GetLogLevelString(logLevel);
			format = format.Replace("{Level}", levelString);
		}
		else
		{
			format = format.Replace("[{Level}]", string.Empty).Replace("{Level}", string.Empty);
		}

		if (options.IncludeEventId && eventId.Id != 0 && format.Contains("{EventId}"))
		{
			format = format.Replace("{EventId}", eventId.ToString());
		}
		else
		{
			format = format.Replace("[{EventId}]", string.Empty).Replace("{EventId}", string.Empty);
		}

		// Handle hyperlinks in message
		if (options.EnableHyperlinks)
		{
			message = FormatHyperlinks(message);
		}

		format = format.Replace("{Message}", message);

		// Handle exception
		if (exception != null)
		{
			var exceptionText = FormatException(exception, options.SingleLineExceptions);
			if (format.Contains("{Exception}"))
			{
				format = format.Replace("{Exception}", exceptionText);
			}
			else
			{
				format += Environment.NewLine + exceptionText;
			}
		}
		else
		{
			format = format.Replace("{Exception}", string.Empty);
		}

		// Clean up extra spaces
		format = CleanupFormat(format);

		return format;
	}

	private static string GetLogLevelString(LogLevel logLevel) => logLevel switch
	{
		LogLevel.Trace => "TRCE",
		LogLevel.Debug => "DBUG", 
		LogLevel.Information => "INFO",
		LogLevel.Warning => "WARN",
		LogLevel.Error => "FAIL",
		LogLevel.Critical => "CRIT",
		LogLevel.None => "NONE",
		_ => logLevel.ToString().ToUpperInvariant()
	};

	private static string FormatHyperlinks(string message)
	{
		// Convert URLs to clickable hyperlinks using ANSI escape sequences
		message = UrlRegex.Replace(message, match =>
		{
			var url = match.Groups[1].Value;
			return CreateHyperlink(url, url);
		});

		// Convert file paths to clickable hyperlinks (if they exist)
		message = FilePathRegex.Replace(message, match =>
		{
			var path = match.Groups[1].Value;
			if (File.Exists(path) || Directory.Exists(path))
			{
				return CreateHyperlink($"file:///{path.Replace('\\', '/')}", path);
			}
			return path;
		});

		return message;
	}

	private static string CreateHyperlink(string url, string text)
	{
		// OSC 8 hyperlink format: \e]8;;URL\e\\TEXT\e]8;;\e\\
		return $"\u001b]8;;{url}\u001b\\{text}\u001b]8;;\u001b\\";
	}

	private static string FormatException(Exception exception, bool singleLine)
	{
		if (singleLine)
		{
			return $"{exception.GetType().Name}: {exception.Message}";
		}

		var sb = new StringBuilder();
		sb.AppendLine($"Exception: {exception.GetType().Name}");
		sb.AppendLine($"Message: {exception.Message}");
		
		if (!string.IsNullOrEmpty(exception.StackTrace))
		{
			sb.AppendLine("Stack Trace:");
			sb.AppendLine(exception.StackTrace);
		}

		// Handle inner exceptions
		var innerException = exception.InnerException;
		while (innerException != null)
		{
			sb.AppendLine($"Inner Exception: {innerException.GetType().Name}");
			sb.AppendLine($"Message: {innerException.Message}");
			innerException = innerException.InnerException;
		}

		return sb.ToString().TrimEnd();
	}

	private static string CleanupFormat(string format)
	{
		// Remove multiple consecutive spaces
		while (format.Contains("  "))
		{
			format = format.Replace("  ", " ");
		}

		// Remove leading/trailing spaces
		format = format.Trim();

		// Remove empty brackets
		format = format.Replace("[]", string.Empty);

		return format;
	}
}
