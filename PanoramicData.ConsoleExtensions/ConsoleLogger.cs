using Microsoft.Extensions.Logging;

namespace PanoramicData.ConsoleExtensions;

/// <summary>
/// A simple console logger implementation that provides colored output based on log levels
/// </summary>
public class ConsoleLogger : ILogger
{
	private readonly ConsoleLoggerOptions _consoleLoggerOptions;

	/// <summary>
	/// Initializes a new instance of the ConsoleLogger with default options
	/// </summary>
	public ConsoleLogger()
	{
		_consoleLoggerOptions = new ConsoleLoggerOptions();
	}

	/// <summary>
	/// Initializes a new instance of the ConsoleLogger with the specified options
	/// </summary>
	/// <param name="consoleLoggerOptions">The logger configuration options</param>
	public ConsoleLogger(ConsoleLoggerOptions consoleLoggerOptions)
	{
		_consoleLoggerOptions = consoleLoggerOptions ?? throw new ArgumentNullException(nameof(consoleLoggerOptions));
	}

	/// <inheritdoc/>
	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter)
	{
		if (!IsEnabled(logLevel))
		{
			return;
		}

		// Format the message using structured logging if enabled
		string formattedMessage;
		if (_consoleLoggerOptions.EnableStructuredLogging)
		{
			formattedMessage = StructuredLogFormatter.FormatMessage(
				logLevel, eventId, state, exception, formatter, _consoleLoggerOptions);
		}
		else
		{
			// Backward compatibility: use original simple formatting
			formattedMessage = formatter.Invoke(state, exception);
		}

		// Apply color formatting
		var oldColor = Console.ForegroundColor;
		try
		{
			Console.ForegroundColor = GetLogLevelColor(logLevel);
			Console.WriteLine(formattedMessage);
		}
		finally
		{
			Console.ForegroundColor = oldColor;
		}
	}

	private ConsoleColor GetLogLevelColor(LogLevel logLevel) => logLevel switch
	{
		LogLevel.Trace => _consoleLoggerOptions.TraceColor,
		LogLevel.Debug => _consoleLoggerOptions.DebugColor,
		LogLevel.Information => _consoleLoggerOptions.InformationColor,
		LogLevel.Warning => _consoleLoggerOptions.WarningColor,
		LogLevel.Error => _consoleLoggerOptions.ErrorColor,
		LogLevel.Critical => _consoleLoggerOptions.CriticalColor,
		LogLevel.None => _consoleLoggerOptions.NoneColor,
		_ => throw new ArgumentOutOfRangeException(nameof(logLevel))
	};

	/// <inheritdoc/>
	public bool IsEnabled(LogLevel logLevel)
		=> logLevel >= _consoleLoggerOptions.LogLevel;
		
	/// <inheritdoc/>
	public IDisposable? BeginScope<TState>(TState state) where TState : notnull
		=> throw new NotSupportedException();
}
