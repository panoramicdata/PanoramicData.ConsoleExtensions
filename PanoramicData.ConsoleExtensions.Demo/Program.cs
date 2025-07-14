using Microsoft.Extensions.Logging;
using PanoramicData.ConsoleExtensions;

namespace PanoramicData.ConsoleExtensions.Demo;

/// <summary>
/// Demo application showcasing the PanoramicData.ConsoleExtensions library features
/// </summary>
internal class Program
{
	private static void Main(string[] args)
	{
		// Check if we should run just the logging tests (non-interactive)
		if (args.Length > 0 && args[0].Equals("logging", StringComparison.OrdinalIgnoreCase))
		{
			LoggingTest.RunTests();
			return;
		}

		Console.WriteLine("=== PanoramicData.ConsoleExtensions Demo ===");
		Console.WriteLine();

		// Demo 1: Password Reading
		DemoPasswordReading();

		Console.WriteLine();
		Console.WriteLine("Press any key to continue to logger demo...");
		Console.ReadKey();
		Console.WriteLine();

		// Demo 2: Console Logger with default options
		DemoConsoleLoggerDefault();

		Console.WriteLine();
		Console.WriteLine("Press any key to continue to custom logger demo...");
		Console.ReadKey();
		Console.WriteLine();

		// Demo 3: Console Logger with custom options
		DemoConsoleLoggerCustom();

		Console.WriteLine();
		Console.WriteLine("Press any key to continue to structured logging demo...");
		Console.ReadKey();
		Console.WriteLine();

		// Demo 4: Structured Logging Features
		DemoStructuredLogging();

		Console.WriteLine();
		Console.WriteLine("Press any key to continue to hyperlink demo...");
		Console.ReadKey();
		Console.WriteLine();

		// Demo 5: Hyperlink Support
		DemoHyperlinks();

		Console.WriteLine();
		Console.WriteLine("Demo completed! Press any key to exit...");
		Console.ReadKey();
	}

	private static void DemoPasswordReading()
	{
		Console.WriteLine("--- Password Reading Demo ---");
		Console.WriteLine("The ReadPassword() method allows secure password input with:");
		Console.WriteLine("• Characters are masked with asterisks (*)");
		Console.WriteLine("• Backspace key works for corrections");
		Console.WriteLine("• No password echoing to console");
		Console.WriteLine();

		Console.Write("Enter a test password: ");
		var password = ConsolePlus.ReadPassword();
		Console.WriteLine();
		Console.WriteLine($"You entered a password with {password.Length} characters.");
		Console.WriteLine("(Password content is not displayed for security)");
	}

	private static void DemoConsoleLoggerDefault()
	{
		Console.WriteLine("--- Console Logger Demo (Default Settings) ---");
		Console.WriteLine("Default logger with Information level and standard colors:");
		Console.WriteLine();

		var logger = new ConsoleLogger();

		// These will be displayed (>= Information level)
		logger.LogInformation("This is an informational message");
		logger.LogWarning("This is a warning message");
		logger.LogError("This is an error message");
		logger.LogCritical("This is a critical message");

		// These will not be displayed (< Information level)
		logger.LogTrace("This trace message won't be shown");
		logger.LogDebug("This debug message won't be shown");

		Console.WriteLine();
		Console.WriteLine("Note: Trace and Debug messages are not shown due to default Information log level.");
	}

	private static void DemoConsoleLoggerCustom()
	{
		Console.WriteLine("--- Console Logger Demo (Custom Settings) ---");
		Console.WriteLine("Custom logger with Debug level and custom colors:");
		Console.WriteLine();

		var customOptions = new ConsoleLoggerOptions(LogLevel.Debug)
		{
			TraceColor = ConsoleColor.DarkGray,
			DebugColor = ConsoleColor.Blue,
			InformationColor = ConsoleColor.Green,
			WarningColor = ConsoleColor.DarkYellow,
			ErrorColor = ConsoleColor.Red,
			CriticalColor = ConsoleColor.White
		};

		var customLogger = new ConsoleLogger(customOptions);

		// All of these will be displayed (>= Debug level)
		customLogger.LogTrace("This is a trace message with custom color");
		customLogger.LogDebug("This is a debug message with custom color");
		customLogger.LogInformation("This is an informational message with custom color");
		customLogger.LogWarning("This is a warning message with custom color");
		customLogger.LogError("This is an error message with custom color");
		customLogger.LogCritical("This is a critical message with custom color");

		Console.WriteLine();
		Console.WriteLine("All log levels are shown because we set LogLevel.Debug as minimum.");

		// Demo with structured logging
		Console.WriteLine();
		Console.WriteLine("--- Structured Logging Example ---");
		var userName = "JohnDoe";
		var loginTime = DateTime.Now;
		customLogger.LogInformation("User {UserName} logged in at {LoginTime:yyyy-MM-dd HH:mm:ss}", 
			userName, loginTime);

		try
		{
			// Simulate an exception
			throw new InvalidOperationException("This is a demo exception");
		}
		catch (Exception ex)
		{
			customLogger.LogError(ex, "An error occurred during demo execution");
		}
	}

	private static void DemoStructuredLogging()
	{
		Console.WriteLine("--- Structured Logging Demo ---");
		Console.WriteLine("Enhanced logger with timestamps, event IDs, and custom formatting:");
		Console.WriteLine();

		var structuredOptions = new ConsoleLoggerOptions(LogLevel.Trace)
		{
			IncludeTimestamp = true,
			IncludeEventId = true,
			IncludeLogLevel = true,
			OutputFormat = "{Timestamp} [{Level}] ({EventId}) {Message}",
			InformationColor = ConsoleColor.Green,
			WarningColor = ConsoleColor.Yellow,
			ErrorColor = ConsoleColor.Red
		};

		var structuredLogger = new ConsoleLogger(structuredOptions);

		// Demonstrate structured logging with various event IDs
		structuredLogger.LogTrace(new EventId(1001, "UserAction"), "User {UserName} performed action {Action}", "Alice", "Login");
		structuredLogger.LogDebug(new EventId(1002, "Database"), "Database query executed in {ElapsedMs}ms", 42);
		structuredLogger.LogInformation(new EventId(1003, "Request"), "HTTP {Method} request to {Url} completed with status {StatusCode}", 
			"GET", "/api/users", 200);
		structuredLogger.LogWarning(new EventId(2001, "Performance"), "Slow query detected: {QueryTime}ms for query {Query}", 
			1500, "SELECT * FROM large_table");
		structuredLogger.LogError(new EventId(3001, "Security"), "Failed login attempt for user {UserName} from IP {IPAddress}", 
			"admin", "192.168.1.100");

		Console.WriteLine();
		Console.WriteLine("Notice the consistent formatting with timestamps, log levels, and event IDs.");

		// Demo exception formatting
		Console.WriteLine();
		Console.WriteLine("--- Exception Formatting ---");
		try
		{
			throw new InvalidOperationException("This is a structured logging exception demo")
			{
				Data = { { "UserId", 12345 }, { "Operation", "ProcessPayment" } }
			};
		}
		catch (Exception ex)
		{
			structuredLogger.LogError(new EventId(3002, "ProcessingError"), ex, 
				"Payment processing failed for user {UserId} with amount {Amount:C}", 12345, 99.99m);
		}

		// Demo with single-line exception format
		Console.WriteLine();
		Console.WriteLine("--- Compact Exception Format ---");
		var compactOptions = new ConsoleLoggerOptions(LogLevel.Error)
		{
			IncludeTimestamp = true,
			SingleLineExceptions = true,
			OutputFormat = "{Timestamp} [{Level}] {Message}",
			ErrorColor = ConsoleColor.DarkRed
		};

		var compactLogger = new ConsoleLogger(compactOptions);
		try
		{
			throw new ArgumentException("Invalid parameter value", "userId");
		}
		catch (Exception ex)
		{
			compactLogger.LogError(ex, "Validation failed for user input");
		}
	}

	private static void DemoHyperlinks()
	{
		Console.WriteLine("--- Hyperlink Support Demo ---");
		Console.WriteLine("Logger can automatically detect and make URLs and file paths clickable:");
		Console.WriteLine("(Hyperlinks work in terminals that support OSC 8 escape sequences)");
		Console.WriteLine();

		var hyperlinkOptions = new ConsoleLoggerOptions(LogLevel.Information)
		{
			IncludeTimestamp = false,
			IncludeLogLevel = true,
			EnableHyperlinks = true,
			InformationColor = ConsoleColor.Cyan,
			WarningColor = ConsoleColor.Yellow
		};

		var hyperlinkLogger = new ConsoleLogger(hyperlinkOptions);

		// Demo URL hyperlinks
		hyperlinkLogger.LogInformation("Visit our documentation at https://github.com/panoramicdata/PanoramicData.ConsoleExtensions");
		hyperlinkLogger.LogInformation("API reference: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging");
		hyperlinkLogger.LogInformation("Multiple URLs: Check https://github.com and https://stackoverflow.com for help");

		// Demo file path hyperlinks (these will only be clickable if the files exist)
		var currentDir = Environment.CurrentDirectory;
		var projectFile = Path.Combine(currentDir, "PanoramicData.ConsoleExtensions.Demo.csproj");
		var programFile = Path.Combine(currentDir, "Program.cs");
		
		hyperlinkLogger.LogInformation("Project file: {ProjectFile}", projectFile);
		hyperlinkLogger.LogInformation("Source file: {SourceFile}", programFile);
		
		// Demo with disabled hyperlinks for comparison
		Console.WriteLine();
		Console.WriteLine("--- Hyperlinks Disabled ---");
		var noHyperlinkOptions = new ConsoleLoggerOptions(LogLevel.Information)
		{
			EnableHyperlinks = false,
			IncludeLogLevel = false
		};

		var noHyperlinkLogger = new ConsoleLogger(noHyperlinkOptions);
		noHyperlinkLogger.LogInformation("Same URL without hyperlink: https://github.com/panoramicdata/PanoramicData.ConsoleExtensions");

		Console.WriteLine();
		Console.WriteLine("Note: Hyperlink support depends on your terminal application.");
		Console.WriteLine("Windows Terminal, VS Code terminal, and many Linux terminals support clickable links.");
	}
}
