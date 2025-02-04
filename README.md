# DefenderTool (Translated to english)

DefenderTool is a Windows utility application that allows you to manage Windows Defender and other security mitigations on your system with ease. With a simple graphical interface, you can enable or disable Windows Defender, remove it entirely, or disable all security mitigations.

> **Warning:** This tool performs critical system modifications that can weaken your system’s security. **Use at your own risk** and ensure you understand the implications before running any payloads. It is highly recommended to create a system restore point prior to use.

## Table of Contents

- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Code Overview](#code-overview)
  - [MainForm.cs](#mainformcs)
  - [Program.cs](#programcs)
- [Security Warning](#security-warning)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgments](#acknowledgments)
- [GitHub Repository](#github-repository)

## Features

- **Enable Windows Defender:** Easily activate Windows Defender if it has been disabled.
- **Disable Windows Defender:** Quickly deactivate Windows Defender.
- **Remove Windows Defender:** Permanently remove Windows Defender from your system.
- **Disable All Security Mitigations:** Turn off various security measures (e.g., UAC, administrative safeguards).
- **Automatic Update Checking:** On startup, the tool checks for a new version and prompts you if an update is available.
- **Resource Extraction:** Embedded scripts and executables are automatically extracted from the application resources to the system's temporary folder.
- **Real-Time Logging:** The application logs output and error messages from executed scripts, providing transparency during operations.
- **User-Friendly Interface:** Built using Windows Forms, the interface is simple and intuitive.

## Requirements

- **Operating System:** Windows 7 or later.
- **.NET Framework:** .NET Framework 4.6 or higher *(or .NET Core/5/6, depending on your build configuration)*.
- **Administrative Privileges:** Running the application as an administrator is required for most operations.

## Installation

1. **Clone the Repository**

   Open a terminal or command prompt and clone the repository:

   ```bash
   git clone https://github.com/danbenba/DefenderTool.git
   ```

2. **Open the Project**

   Open the `DefenderTool.sln` solution file in [Visual Studio](https://visualstudio.microsoft.com/) or your preferred C# IDE.

3. **Build the Project**

   Build the solution using Visual Studio (`Build > Build Solution`).

4. **Run the Application**

   Run the application. For full functionality (modifying Windows Defender and security settings), make sure to run it as an administrator.

## Usage

1. **Select a Payload**

   On the main window, use the dropdown menu to select one of the available actions:
   
   - **Enable W-Defender**
   - **Disable W-Defender**
   - **Remove Defender**
   - **Disable All Security Mitigations**

2. **Apply the Action**

   Click the `Apply` button after selecting your desired payload. A confirmation dialog will appear to ensure you want to proceed.

3. **Review Logs**

   During execution, logs are displayed in the text area. These logs provide details about the script’s output and any errors encountered.

4. **Restart Option**

   If you want your PC to restart automatically after the action, check the "Restart PC after execution" checkbox before clicking `Apply`.

5. **Additional Options**

   - **System Information:** Click the `System Info` button to view operating system details and .NET runtime information.
   - **About:** Click the `About` button to view more information about the application.

## Code Overview

The application consists of two primary components:

### MainForm.cs

- **User Interface:**  
  Implements the Windows Form containing:
  - A **title label** displaying the tool’s name and version.
  - A **combo box** (`comboBoxScripts`) listing the payload options.
  - An **Apply button** (`btnApply`) to execute the selected action.
  - A **checkbox** (`checkBoxRestart`) to opt for an automatic system restart after execution.
  - A **rich text box** (`richTextBoxLogs`) for logging script outputs and errors.
  - Additional buttons (`btnSystemInfo`, `btnAbout`) to display system information and application details.

- **Resource Handling:**  
  - **LoadIconFromFile():** Loads the application icon from an extracted resource.
  - **Resource Extraction:** Before any actions are performed, the required scripts and executables (e.g., `enable.bat`, `disable.bat`, `defrv.exe`) are extracted from the embedded resources to the system's temporary folder.

- **Script Execution:**  
  - When a payload is selected and the `Apply` button is clicked, the application determines the correct script to run using the `GetScriptFilePath()` method.
  - Depending on the file type (batch or executable), the application creates a `ProcessStartInfo` instance to execute the script.
  - Real-time output and error streams are captured and displayed in the logs.
  - If the script executes successfully and the restart option is checked, the application will trigger a system reboot.

### Program.cs

- **Version Checking:**
  - On startup, the application checks for the latest version by comparing a remote version string (fetched from GitHub) with the current version.
  - If a newer version is available, the user is prompted to visit the release page for an update.

- **Resource Extraction:**
  - The `ExtractResources()` method extracts necessary embedded files (scripts and icon) into the temporary folder to ensure they are available for execution.

- **Application Startup:**
  - After checking for updates and extracting the resources, the main form is launched.

## Security Warning

**Important:** This tool makes significant changes to your system by altering Windows Defender settings and disabling security features. Misuse or accidental execution may leave your computer vulnerable. Always ensure you understand each action’s impact, and consider creating a backup or system restore point before proceeding.

## Contributing

Contributions are welcome! If you have suggestions, improvements, or bug fixes:
1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Submit a pull request.

Please follow the repository’s coding standards and include appropriate tests for your changes.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

- **danbenba:** For creating and maintaining DefenderTool.
- Thanks to all contributors and testers who have helped improve this project.

## GitHub Repository

For more information, to report issues, or to contribute, please visit the [DefenderTool GitHub repository](https://github.com/danbenba/DefenderTool).
