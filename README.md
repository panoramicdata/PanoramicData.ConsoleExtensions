# PanoramicData.ConsoleExtensions
Extensions for the System.Console namespace

install-package PanoramicData.ConsoleExtensions

## ConsolePlus.ReadPassword

This method provides a simple and safe way to read a password from the command line, including permitting the user to use the backspace key.

```
using PanoramicData.ConsoleExtensions;
```
...
```
Console.Write("Password: ");
var password = ConsolePlus.ReadPassword();
Console.WriteLine();
```

## ConsoleLogger

This class provides a simple ILogger implementation for the command line.
It implements colored logging, with control over the color used for each log level and sensible defaults.

### Basic Usage

```csharp
using PanoramicData.ConsoleExtensions;
```
...
```csharp
var consoleLogger1 = new ConsoleLogger();
consoleLogger1.LogInformation("The Date is {DateTime:yyyy-MM-dd}", DateTime.UtcNow);

var consoleLogger2 = new ConsoleLogger(new ConsoleLoggerOptions
	{
		TraceColor = ConsoleColor.DarkYellow,
		LogLevel = LogLevel.Debug
	});
consoleLogger2.LogTrace("The Date is {DateTime:yyyy-MM-dd}", DateTime.UtcNow);
consoleLogger2.LogError(exception, "Failure occurred at {DateTime:yyyy-MM-dd}", DateTime.UtcNow);
```

### Structured Logging Features

The ConsoleLogger supports advanced structured logging features while maintaining backward compatibility:

#### Timestamps and Custom Formatting
```csharp
var options = new ConsoleLoggerOptions
{
    IncludeTimestamp = true,
    IncludeEventId = true,
    TimeFormat = "yyyy-MM-dd HH:mm:ss.fff",
    OutputFormat = "{Timestamp} [{Level}] ({EventId}) {Message}"
};
var logger = new ConsoleLogger(options);
logger.LogInformation(new EventId(1001), "User {UserName} logged in", "Alice");
```

#### Hyperlink Support
URLs and file paths are automatically detected and made clickable in supported terminals:

```csharp
var options = new ConsoleLoggerOptions { EnableHyperlinks = true };
var logger = new ConsoleLogger(options);
logger.LogInformation("Visit https://github.com/panoramicdata/PanoramicData.ConsoleExtensions");
logger.LogInformation("Error in file: C:\\Source\\MyProject\\Program.cs");
```

#### Exception Formatting
Choose between detailed multi-line or compact single-line exception formatting:

```csharp
var options = new ConsoleLoggerOptions { SingleLineExceptions = true };
var logger = new ConsoleLogger(options);
logger.LogError(exception, "Operation failed");
```

## Demo Application

A comprehensive demo application is included in the `PanoramicData.ConsoleExtensions.Demo` project that showcases all the library features:

- Interactive password reading demonstration
- Default console logger with standard colors and log levels
- Custom console logger with custom colors and log level filtering
- Structured logging examples
- Exception logging examples

To run the demo:

```bash
cd PanoramicData.ConsoleExtensions.Demo
dotnet run
```

Or use the VS Code launch configuration "Run Demo" for easy debugging.

## Publishing to NuGet

A PowerShell script `publish.ps1` is provided for easy publishing to NuGet.org:

### Setup
1. Obtain a NuGet API key from [https://www.nuget.org/account/apikeys](https://www.nuget.org/account/apikeys)
2. Replace the contents of `nuget.key` with your actual API key (the file should contain only the key)
3. Run the publish script

### Usage
```powershell
# Publish with default settings (Release configuration)
.\publish.ps1

# Specify custom configuration
.\publish.ps1 -Configuration Debug

# Use a different key file
.\publish.ps1 -KeyFile "my-api-key.txt"

# Auto-publish without confirmation prompt
.\publish.ps1 -SkipConfirmation
```

**Windows Users**: You can also use `publish.bat` which bypasses PowerShell execution policy restrictions.

The script will:
- Validate the API key file exists and contains a valid key
- Build the solution in the specified configuration  
- Create the NuGet package
- Prompt for confirmation before publishing
- Publish to NuGet.org

**Security Note**: The `nuget.key` file is automatically excluded from Git via `.gitignore` to protect your API key.
