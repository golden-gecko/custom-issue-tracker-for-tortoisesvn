@echo off
C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\RegAsm.exe bin\Release\CustomIssueTracker.dll /codebase /regfile:CustomIssueTracker.reg
echo.>> CustomIssueTracker.reg
echo [HKEY_CLASSES_ROOT\CLSID\{5870B3F1-8393-4C83-ACED-1D5E803A4F2B}\Implemented Categories\{3494FA92-B139-4730-9591-01135D5E7831}]>> CustomIssueTracker.reg
pause
