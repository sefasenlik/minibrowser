# MiniTranslate - Installation & User Guide

A lightweight utility for instant, hotkey-based text translation.

## Quick Start

1.  **Download**: Go to the [Releases](../../releases) page and download the latest `MiniTranslate-vX.X.zip` file.
2.  **Extract**: Extract the `publish` folder from the `.zip` file to a permanent location on your computer (e.g., `C:\Program Files\MiniTranslate`).
3.  **Run**: Double-click `MiniTranslate.exe` to start the application.
4.  **Tray Icon**: A blue "T" icon will appear in your system tray (bottom-right corner of your screen).

## How to Use

1.  **Copy Text**: Highlight text in any application (your browser, a PDF, a code editor, etc.) and press `Ctrl+C`.
2.  **Press Hotkey**: Press the global hotkey (default is `Ctrl+Q`).
3.  **Get Translation**: A clean, minimal browser window will pop up with the translated text.

## Configuration

All settings can be configured by right-clicking the tray icon.

### Quick Settings (via Right-Click Menu)

For fast adjustments, you can change the most common settings directly from the tray menu:
- **Translator**: Switch between `Google Translate` and `Yandex Translate`.
- **Browser**: Choose whether to open translations in `Chrome`, `Edge`, or your `Default` system browser.
- **Source Language**: Set the language you are translating *from*.
- **Target Language**: Set the language you want to translate *to*.

Your selection is saved instantly.

### Main Settings (Settings Window)

For more options, right-click the tray icon and select **"Settings"**:
- **Global Hotkey**: Configure the keyboard shortcut that triggers the translation.
- **Browser Window Size**: Set the default width and height of the translation window.
- **Run at Windows startup**: Check this box to have MiniTranslate start automatically when you log in to Windows.
- **Test Button**: A "Test Translation" button is available to verify your settings with text from your clipboard.

## Troubleshooting

### Hotkey Not Working?
- Another application might be using the same key combination. Try choosing a different hotkey in the Settings window.
- Make sure at least one modifier key (Ctrl, Alt, Shift, Win) is selected.

### Can't Find the Tray Icon?
- Look in the system tray overflow area by clicking the small `^` arrow in the bottom-right corner of your screen.

### Translation Not Working?
- Make sure you have copied text to your clipboard *before* pressing the hotkey.
- In the Settings window, use the "Test Translation" button to see if it's a clipboard issue or a settings issue.
- Verify that both a source and target language are selected.

### Application Won't Start?
- Check your system tray to see if it's already running. Only one instance of the application can run at a time.

## Uninstalling

1.  If the application is running, right-click the tray icon and select "Exit".
2.  Delete the folder where you extracted the application files.
3.  To remove saved settings, delete the folder at `%APPDATA%\MiniTranslate`.
4.  If you enabled "Run at Windows startup," open the Settings window and uncheck the box before exiting the application to remove the registry key.

## Features

✅ **Global Hotkeys** - Works from any application
✅ **Customizable** - Change URL and keyboard shortcuts
✅ **System Tray** - Runs quietly in background
✅ **Single Instance** - Won't create duplicates
✅ **URL Validation** - Automatically adds HTTPS if needed
✅ **Settings Persistence** - Remembers your configuration
✅ **App Mode** - Opens websites in minimal browser windows (Chrome/Edge)
✅ **Clean Interface** - No toolbars, address bars, or browser UI clutter
✅ **Auto Translation** - Instant clipboard text translation using Yandex Translate
✅ **Dual Mode** - Switch between website launcher and translator functionality
✅ **Multi-Language** - Support for 70+ languages including English, Russian, Spanish, etc.

## Configuration File

Settings are stored in: `%APPDATA%\MiniTranslate\settings.json`

Example configurations:

**Translation Mode:**
```json
{
  "WebsiteUrl": "https://www.google.com",
  "HotkeyModifiers": 2,
  "HotkeyKey": 81,
  "SourceLanguage": "en",
  "TargetLanguage": "ru",
  "UseTranslateMode": true
}
```

**Website Mode:**
```json
{
  "WebsiteUrl": "https://gmail.com",
  "HotkeyModifiers": 1,
  "HotkeyKey": 71,
  "UseTranslateMode": false
}
```

## Technical Details

- **Framework**: .NET 6.0 Windows Forms
- **Size**: ~139MB (self-contained executable)
- **Requirements**: Windows 10/11 (no .NET installation required)
- **Memory Usage**: ~50MB RAM when running

## Popular Hotkey Combinations

### Translation Mode (Default)
- **Ctrl+Q**: Quick translate (default)
- **Alt+T**: Translate text
- **Ctrl+Shift+T**: Translation hotkey
- **Win+T**: Windows + Translate

### Custom Website Mode  
- **Alt+G**: Gmail
- **Ctrl+Shift+Y**: YouTube  
- **Win+B**: Bookmarks/favorite site
- **Alt+N**: News site
- **Ctrl+Alt+M**: Email

## Popular Language Combinations

- **English → Russian** (en → ru)
- **English → Spanish** (en → es)  
- **Russian → English** (ru → en)
- **Chinese → English** (zh → en)
- **French → English** (fr → en)
- **German → English** (de → en)
- **Japanese → English** (ja → en)

---

**Need Help?** Check the README.md file for more technical information. 