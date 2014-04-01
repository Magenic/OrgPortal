OrgPortal
=========

Portal for finding and installing WinRT business apps via side-loading.

See the [OrgPortal wiki](https://github.com/Magenic/OrgPortal/wiki) for more information.

This solution includes three parts:

1. A WinRT client app so the user can browse and install your business apps
1. An ASP.NET service providing metadata and the appx files for your apps
1. A Windows Desktop system tray app that performs the actual install via PowerShell

The WinRT app and system tray app interact with each other via the file system. When the user wants to install an app, the WinRT portal creates a request file with the URL to the appx file on the server. The system tray app monitors the same folder for request files and when a request file is discovered it downloads the appx file and installs it by executing a PowerShell command. The results (success or fail) are written to a log file so the WinRT app can display status to the user.

The end user deploys OrgPortal via ClickOnce from the ASP.NET server. The ClickOnce installer deploys the system tray app, which will automatically install the WinRT client app.

Apps can be designated as _auto-install_, meaning the system tray app will automatically install the app. Or they can be designated as _auto-update_, meaning the app won't automatically install, but the system tray app will automatically update the app when newer versions become available on the server. By default the system tray app checks for new apps and/or versions roughly once per day.

The WinRT client app displays content from a URL, allowing the app to function as an informational portal as well as a private store. It also lists private apps available for installation based on information provided by the ASP.NET server.

The system tray app provides a set of admin related functionality, including:

* Enter and activate a side-loading unlock key
* Get a temporary development/test unlock key
* Show a log of activity

The web site provides a set of admin related functionality, including:

* Upload appx files to server so they are available for deployment to clients
* Set the auto-install/auto-update designation for each appx
* Remove an appx from the server, preventing future installs to clients

**Note:** This solution does not bypass the need to unlock your device for side-loading, or alter the requirement for the appx package to be signed using a trusted certificate. You will need to meet those two requirements before OrgPortal will function on a device.

**Note:** OrgPortal only works with Intel-based devices. There is no way to run a system tray app on an ARM device, so they are not supported by this solution.
