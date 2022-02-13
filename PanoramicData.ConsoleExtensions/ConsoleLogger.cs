using Microsoft.Extensions.Logging;

namespace PanoramicData.ConsoleExtensions
{
	public class ConsoleLogger : ILogger
	{
		private readonly ConsoleLoggerOptions _consoleLoggerOptions;

		public ConsoleLogger(ConsoleLoggerOptions consoleLoggerOptions)
		{
			_consoleLoggerOptions = consoleLoggerOptions;
		}

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception? exception,
			Func<TState, Exception, string> formatter)
		{
			var oldColor = Console.ForegroundColor;
			Console.ForegroundColor = logLevel switch
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
			Console.WriteLine(formatter.Invoke(state, exception ?? null!));
			Console.ForegroundColor = oldColor;
		}

		public bool IsEnabled(LogLevel logLevel)
			=> logLevel >= _consoleLoggerOptions.LogLevel;
		public IDisposable BeginScope<TState>(TState state)
			=> throw new NotSupportedException();
	}
}
