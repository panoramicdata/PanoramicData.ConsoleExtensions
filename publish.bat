@echo off
REM Windows batch wrapper for publish.ps1
REM This allows running the PowerShell script even with restricted execution policies

echo PanoramicData.ConsoleExtensions Publisher (Batch Wrapper)
echo =========================================================
echo.
echo This will run the PowerShell publish script...
echo.

powershell -ExecutionPolicy Bypass -File "%~dp0publish.ps1" %*

if %ERRORLEVEL% neq 0 (
    echo.
    echo Error: The publish script failed with exit code %ERRORLEVEL%
    pause
    exit /b %ERRORLEVEL%
)

echo.
echo Press any key to exit...
pause >nul
