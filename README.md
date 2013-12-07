OrgPortal
=========

Portal for finding and installing WinRT business apps via side-loading.

This solution includes three parts:

1. A WinRT client app so the user can browse and install your business apps
1. An ASP.NET service providing metadata and the appx files for your apps
1. A Windows Desktop system tray app that performs the actual install via PowerShell

The WinRT app and system tray app interact with each other via the file system. When the user wants to install an app, the WinRT portal creates a request file with the URL to the appx file on the server. The system tray app monitors the same folder for request files and when a request file is discovered it downloads the appx file and installs it by executing a PowerShell command. The results (success or fail) are written to a log file so the WinRT app can display status to the user.

This solution does not bypass the need to unlock your device for side-loading, or alter the requirement for the appx package to be signed using a trusted certificate. You will need to meet those two requirements before OrgPortal will function on a device.