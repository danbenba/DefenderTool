::[Bat To Exe Converter]
::
::YAwzoRdxOk+EWAnk
::fBw5plQjdG8=
::YAwzuBVtJxjWCl3EqQJgSA==
::ZR4luwNxJguZRRnk
::Yhs/ulQjdF+5
::cxAkpRVqdFKZSDk=
::cBs/ulQjdF+5
::ZR41oxFsdFKZSDk=
::eBoioBt6dFKZSDk=
::cRo6pxp7LAbNWATEpCI=
::egkzugNsPRvcWATEpCI=
::dAsiuh18IRvcCxnZtBJQ
::cRYluBh/LU+EWAnk
::YxY4rhs+aU+JeA==
::cxY6rQJ7JhzQF1fEqQJQ
::ZQ05rAF9IBncCkqN+0xwdVs0
::ZQ05rAF9IAHYFVzEqQJQ
::eg0/rx1wNQPfEVWB+kM9LVsJDGQ=
::fBEirQZwNQPfEVWB+kM9LVsJDGQ=
::cRolqwZ3JBvQF1fEqQJQ
::dhA7uBVwLU+EWDk=
::YQ03rBFzNR3SWATElA==
::dhAmsQZ3MwfNWATElA==
::ZQ0/vhVqMQ3MEVWAtB9wSA==
::Zg8zqx1/OA3MEVWAtB9wSA==
::dhA7pRFwIByZRRnk
::Zh4grVQjdCyDJGyX8VAjFD9VQh2+GG6pDaET+NTM5uSyskkcQOcLd4DX5oaLM+8G+EbhZqoo2HtYlM5CCQNdHg==
::YB416Ek+ZG8=
::
::
::978f952a14a936cc963da21a135fa983
@echo off
title Defender Tool - Enable Defender
echo.
echo =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
echo        Enable Windows Defender
echo           v1.2 - by danbenba
echo =-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
echo.
echo USE AT OWN RISK AS IS WITHOUT WARRANTY OF ANY KIND !!!!!
echo.
echo [+] Installing Playload...

rem https://www.defenderui.com
rem https://www.microsoft.com/en-us/wdsi/defenderupdates
rem https://docs.microsoft.com/en-us/powershell/module/defender/set-mppreference?view=windowsserver2022-ps
rem https://docs.microsoft.com/en-us/microsoft-365/security/defender-endpoint/customize-exploit-protection?view=o365-worldwide
rem https://support.microsoft.com/en-us/topic/microsoft-defender-update-for-windows-operating-system-installation-images-1c89630b-61ff-00a1-04e2-2d1f3865450d

reg delete "HKLM\Software\Policies\Microsoft\Windows Defender" /f
reg add "HKLM\Software\Microsoft\Windows\CurrentVersion\Run" /v "SecurityHealth" /t REG_EXPAND_SZ /d "\"%windir%\system32\SecurityHealthSystray.exe\"" /f

rem Restore shell
reg add "HKLM\Software\Classes\*\shellex\ContextMenuHandlers\EPP" /ve /t REG_SZ /d "{09A47860-11B0-4DA5-AFA5-26D86198A780}" /f
reg add "HKLM\Software\Classes\Drive\shellex\ContextMenuHandlers\EPP" /ve /t REG_SZ /d "{09A47860-11B0-4DA5-AFA5-26D86198A780}" /f
reg add "HKLM\Software\Classes\Directory\shellex\ContextMenuHandlers\EPP" /ve /t REG_SZ /d "{09A47860-11B0-4DA5-AFA5-26D86198A780}" /f

rem Enable services
reg add "HKLM\System\CurrentControlSet\Services\BFE" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\MDCoreSvc" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\MpsSvc" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\SgrmBroker" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\WdBoot" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\WdFilter" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\WdNisDrv" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\WdNisSvc" /v "Start" /t REG_DWORD /d "2" /f
reg add "HKLM\System\CurrentControlSet\Services\WinDefend" /v "Start" /t REG_DWORD /d "2" /f

rem 1 - Enable Logging
reg add "HKLM\System\CurrentControlSet\Control\WMI\Autologger\DefenderApiLogger" /v "Start" /t REG_DWORD /d "1" /f
reg add "HKLM\System\CurrentControlSet\Control\WMI\Autologger\DefenderAuditLogger" /v "Start" /t REG_DWORD /d "1" /f

rem Enable Tasks
schtasks /Change /TN "Microsoft\Windows\ExploitGuard\ExploitGuard MDM policy Refresh" /Enable
schtasks /Change /TN "Microsoft\Windows\Windows Defender\Windows Defender Cache Maintenance" /Enable
schtasks /Change /TN "Microsoft\Windows\Windows Defender\Windows Defender Cleanup" /Enable
schtasks /Change /TN "Microsoft\Windows\Windows Defender\Windows Defender Scheduled Scan" /Enable
schtasks /Change /TN "Microsoft\Windows\Windows Defender\Windows Defender Verification" /Enable

rem CloudExtendedTimeout / 1 - 50 / block a suspicious file for up to 60 seconds (Default is 10)
rem reg add "HKLM\Software\Policies\Microsoft\Windows Defender\MpEngine" /v "MpBafsExtendedTimeout" /t REG_DWORD /d "0" /f

rem CloudBlockLevel / 0 - Default / 2 - High / 4 - High+ / 6 - Zero tolerance (block all unknown executables)
rem reg add "HKLM\Software\Policies\Microsoft\Windows Defender\MpEngine" /v "MpCloudBlockLevel" /t REG_DWORD /d "0" /f

rem 1 - Potentially Unwanted Application protection (PUP) is enabled, the applications with unwanted behavior will be blocked at download and install-time
reg add "HKLM\Software\Policies\Microsoft\Windows Defender\MpEngine" /v "MpEnablePus" /t REG_DWORD /d "1" /f

rem Block at First Sight / 0 - Enable / 1 - Disable
reg add "HKLM\Software\Policies\Microsoft\Windows Defender\SpyNet" /v "DisableBlockAtFirstSeen" /t REG_DWORD /d "0" /f

rem Cloud-based Protection / 0 - Disable / 1 - Basic / 2 - Advanced
reg add "HKLM\Software\Policies\Microsoft\Windows Defender\SpyNet" /v "SpynetReporting" /t REG_DWORD /d "1" /f

rem Send file samples when further analysis is required / 0 - Always prompt / 1 - Send safe samples automatically / 2 - Never send / 3 - Send all samples automatically
reg add "HKLM\Software\Policies\Microsoft\Windows Defender\SpyNet" /v "SubmitSamplesConsent" /t REG_DWORD /d "1" /f

rem reg add "HKLM\Software\Microsoft\Windows Defender\Features" /v "TamperProtection" /t REG_DWORD /d "1" /f

rem To prevent using too much CPU, add this file to the exclusion list:
rem C:\Program Files\Windows Defender\MsMpEng.exe
rem reg add "HKLM\Software\Microsoft\Windows Defender\Exclusions\Paths" /v "C:\Program Files\Windows Defender\MsMpEng.exe" /t REG_DWORD /d "0" /f
echo.
echo Playload Installed !
echo.
echo Restart Required.