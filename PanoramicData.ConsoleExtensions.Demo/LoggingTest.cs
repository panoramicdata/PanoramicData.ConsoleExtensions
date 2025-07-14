using Microsoft.Extensions.Logging;
using PanoramicData.ConsoleExtensions;

namespace PanoramicData.ConsoleExtensions.Demo;

/// <summary>
/// Simple test program to showcase structured logging features without interactive input
/// </summary>
internal static class LoggingTest
{
	public static void RunTests()
	{
		Console.WriteLine("=== PanoramicData.ConsoleExtensions Logging Features Test ===");
		Console.WriteLine();

		TestBasicLogging();
		Console.WriteLine();
		
		TestStructuredLogging();
		Console.WriteLine();
		
		TestHyperlinks();
		Console.WriteLine();
		
		TestBackwardCompatibility();
		
		Console.WriteLine();
		Console.WriteLine("All tests completed!");
	}

	private static void TestBasicLogging()
	{
		Console.WriteLine("--- Basic Logging Test ---");
		var logger = new ConsoleLogger();
		
		logger.LogInformation("Basic information message");
		logger.LogWarning("Basic warning message");
		logger.LogError("Basic error message");
	}

	private static void TestStructuredLogging()
	{
		Console.WriteLine("--- Structured Logging Test ---");
		var options = new ConsoleLoggerOptions(LogLevel.Trace)
		{
			IncludeTimestamp = true,
			IncludeEventId = true,
			IncludeLogLevel = true,
			OutputFormat = "{Timestamp} [{Level}] ({EventId}) {Message}",
			InformationColor = ConsoleColor.Green,
			WarningColor = ConsoleColor.Yellow,
			ErrorColor = ConsoleColor.Red
		};

		var logger = new ConsoleLogger(options);
		
		logger.LogTrace(new EventId(1001, "UserAction"), "User {UserName} performed {Action}", "Alice", "Login");
		logger.LogDebug(new EventId(1002, "Database"), "Query executed in {ElapsedMs}ms", 42);
		logger.LogInformation(new EventId(1003, "Request"), "HTTP {Method} {Url} returned {StatusCode}", "GET", "/api/users", 200);
		logger.LogWarning(new EventId(2001, "Performance"), "Slow operation: {Duration}ms", 1500);
		
		// Test exception logging
		try
		{
			throw new InvalidOperationException("Test exception for logging");
		}
		catch (Exception ex)
		{
			logger.LogError(new EventId(3001, "Error"), ex, "Operation failed for user {UserId}", 12345);
		}
	}

	private static void TestHyperlinks()
	{
		Console.WriteLine("--- Hyperlink Test ---");
		var options = new ConsoleLoggerOptions
		{
			EnableHyperlinks = true,
			IncludeLogLevel = true,
			InformationColor = ConsoleColor.Cyan
		};

		var logger = new ConsoleLogger(options);
		
		logger.LogInformation("Visit our repository: https://github.com/panoramicdata/PanoramicData.ConsoleExtensions");
		logger.LogInformation("Documentation at: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging");
		
		// Test file path (current project file should exist)
		var projectFile = Path.Combine(Environment.CurrentDirectory, "PanoramicData.ConsoleExtensions.Demo.csproj");
		logger.LogInformation("Project file: {ProjectFile}", projectFile);
	}

	private static void TestBackwardCompatibility()
	{
		Console.WriteLine("--- Backward Compatibility Test ---");
		var options = new ConsoleLoggerOptions
		{
			EnableStructuredLogging = false // Disable structured logging
		};

		var logger = new ConsoleLogger(options);
		
		logger.LogInformation("This should work exactly like the old version");
		logger.LogWarning("No timestamps, no event IDs, just simple colored text");
		logger.LogError("Backward compatibility maintained!");
	}
}
