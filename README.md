# MiniTranslator - Quick Translation Hotkey

A lightweight Windows utility that lets you instantly translate clipboard text using a global keyboard shortcut. Choose between Google Translate or Yandex Translate and get instant translations from any application.

<img src="https://i.ibb.co/4w3WJvtr/Mini-Translate-Screenshot.png" width="450" />

## Features

- **Global Hotkey**: Works system-wide (default: `Ctrl+Q`).
- **Dual Translators**: Choose between Google Translate and Yandex Translate.
- **Browser Choice**: Launch translations in Chrome, Edge, or your default browser.
- **System Tray Menu**: Runs quietly in the system tray. Right-click the icon to change settings on the fly.
- **Configurable Languages**: Set your default source and target languages.
- **Run at Startup**: Optionally set the application to start with Windows.
- **App Mode**: Opens translations in a clean, minimal browser window.
- **Single Instance**: Prevents multiple copies of the app from running.

## Installation & Usage

1.  **Download**: Go to the [Releases](../../releases) page and download the latest `.zip` file.
2.  **Extract**: Unzip the contents to a folder on your computer.
3.  **Run**: Double-click `MiniTranslator.exe`. The application icon (a blue 'T') will appear in your system tray.

### How to Translate
1.  **Copy Text**: Highlight text in any application and press `Ctrl+C`.
2.  **Press Hotkey**: Press your configured hotkey (e.g., `Ctrl+Q`).
3.  **Get Translation**: A minimal browser window will open with the translation.

### Changing Settings
- **Quick Settings**: Right-click the tray icon to instantly change your Translator, Browser, Source Language, or Target Language.
- **Full Settings**: Right-click the tray icon and select "Settings" to configure the hotkey, window size, and startup options.

## Building from Source

### Prerequisites
- .NET 6.0 SDK or later
- Windows 10/11

### Build Instructions
1.  Clone the repository and open a command prompt in the project directory.
2.  Run the publish command to create a self-contained executable:
    ```bash
    dotnet publish -c Release -r win-x64 --self-contained
    ```
3.  The final application will be located in `bin/Release/net6.0-windows/win-x64/publish/`.

## Configuration
Settings are automatically saved to `%APPDATA%\MiniTranslator\settings.json`. It's recommended to manage these through the application's settings window.

## License

This project is provided as-is for educational and personal use purposes. 
