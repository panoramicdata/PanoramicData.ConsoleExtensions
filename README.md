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

```
using PanoramicData.ConsoleExtensions;
```
...
```
var consoleLogger1 = new ConsoleLogger();
consoleLogger1.LogInformation("The Date is {DateTime:yyyy-MM-dd}", DateTime.UtcNow);

var consoleLogger2 = new ConsoleLogger(new ConsoleLoggerOptions
	{
		TraceColor = ConsoleColor.DarkYellow,
		LogLevel = LogLevel.Debug;
	});
consoleLogger2.LogTrace("The Date is {DateTime:yyyy-MM-dd}", DateTime.UtcNow);
consoleLogger2.LogError(exception, "Failure occurred at {DateTime:yyyy-MM-dd}", DateTime.UtcNow);

```
