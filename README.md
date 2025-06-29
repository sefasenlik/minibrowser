# MiniTranslate - Advanced Translation Utility

A powerful Windows utility that provides instant, high-quality translations using global keyboard shortcuts. MiniTranslate supports multiple translation services including AI-powered ChatGPT, with intelligent language detection and a modern web interface.

<img src="https://gcdnb.pbrd.co/images/38DcE19tdp5j.png" width="450" />

## 🌟 Key Features

### 🔥 Core Functionality
- **Global Hotkey Translation**: Works system-wide with customizable shortcuts (default: `Ctrl+Q`)
- **Multiple Translation Services**: Choose from Google Translate, Yandex Translate, ChatGPT, or custom Translation Server
- **Intelligent Language Detection**: Automatically detects text script and suggests optimal language pairs
- **Auto-Switching Languages**: Smart language switching based on clipboard content analysis

### 🎯 Translation Services
- **Google Translate**: Fast, reliable translations with extensive language support
- **Yandex Translate**: High-quality translations with excellent Russian language support
- **ChatGPT Translator**: AI-powered translations with superior quality and context understanding
- **Translation Server**: Custom server integration with token-based authentication for enterprise use

### 🌐 Browser Integration
- **Multiple Browser Support**: Chrome, Edge, or default browser
- **App Mode**: Clean, minimal browser windows without toolbars
- **Customizable Window Size**: Configurable translation window dimensions
- **Isolated Sessions**: Temporary user data directories for clean browsing

### 🛡️ Privacy & Security
- **Local Processing**: No text sent to external services unless using translation APIs
- **Secure API Integration**: Token-based authentication for custom translation servers
- **Temporary Data**: Browser sessions use isolated temporary directories
- **No Data Collection**: Application doesn't collect or store user data
- **Local Settings**: All configuration stored locally in user's AppData folder

### ⚙️ Advanced Features
- **System Tray Integration**: Runs quietly in background with comprehensive context menu
- **Single Instance Protection**: Prevents multiple app instances
- **Automatic Startup**: Optional Windows startup integration
- **Built-in Web Server**: Local Node.js server for serving translation interfaces
- **Comprehensive Logging**: Detailed logging for debugging and monitoring
- **Hotkey Conflict Detection**: Automatic detection and warning of hotkey conflicts

## 🚀 Installation & Quick Start

### Prerequisites
- Windows 10/11
- .NET 6.0 Runtime (included in self-contained builds)
- Node.js (for local web server functionality)

### Installation
1. **Download**: Get the latest release from the [Releases](../../releases) page
2. **Extract**: Unzip to a permanent location (e.g., `C:\Program Files\MiniTranslate`)
3. **Run**: Double-click `MiniTranslate.exe` - a blue 'T' icon will appear in your system tray

### First Translation
1. **Copy Text**: Select text in any application and press `Ctrl+C`
2. **Press Hotkey**: Use your configured hotkey (default: `Ctrl+Q`)
3. **Get Translation**: A clean browser window opens with your translation

## 🔧 Configuration

### Quick Settings (System Tray)
Right-click the tray icon for instant access to:
- **Translator Selection**: Switch between Google, Yandex, ChatGPT, or Translation Server
- **Language Pairs**: Change source and target languages
- **Browser Choice**: Select Chrome, Edge, or default browser
- **Language Switching**: Quick swap of source/target languages

### Advanced Settings
Access full configuration via **Settings** in the tray menu:
- **Global Hotkey**: Customize keyboard shortcuts with modifier keys
- **Window Dimensions**: Set translation window size
- **Startup Options**: Configure automatic Windows startup
- **API Configuration**: Set up ChatGPT API keys and server URLs
- **Auto-Switching**: Enable/disable intelligent language detection

## 🤖 ChatGPT Integration

### Setup
1. **Get API Key**: Obtain from [OpenAI Platform](https://platform.openai.com/api-keys)
2. **Configure**: Right-click tray icon → Translator → ChatGPT Translator
3. **Enter Key**: Input your API key in the web interface
4. **Start Translating**: Use your hotkey for AI-powered translations

### Features
- **High-Quality Translations**: Context-aware AI translations
- **Modern Interface**: Clean, responsive web-based translator
- **Secure API**: Direct integration with OpenAI's API
- **Language Support**: All major languages supported

For detailed setup instructions, see [CHATGPT_SETUP.md](CHATGPT_SETUP.md).

## 🌍 Language Support

MiniTranslate supports **35+ languages** including:

**European Languages**: English, Russian, Spanish, French, German, Italian, Portuguese, Polish, Dutch, Swedish, Danish, Norwegian, Finnish, Czech, Slovak, Slovenian, Estonian, Latvian, Lithuanian, Hungarian, Romanian, Croatian, Greek

**Asian Languages**: Chinese, Japanese, Korean, Arabic, Hindi, Thai, Vietnamese, Indonesian, Malay

**Other Scripts**: Hebrew

### Intelligent Language Detection
The app automatically detects text scripts (Cyrillic, Latin, Arabic, Chinese, Japanese, Korean, Greek, Hebrew, Thai, Devanagari) and suggests optimal language pairs for translation.

## 🏢 Enterprise Features

### Translation Server Integration
- **Custom Server Support**: Deploy your own translation server
- **Token Authentication**: Secure access with user tokens
- **Admin Panel**: Web-based user management interface
- **Docker Support**: Easy deployment with Docker containers
- **Load Balancing**: Support for multiple server instances

For server setup instructions, see [server/README.md](server/README.md).

## 🔨 Building from Source

### Prerequisites
- .NET 6.0 SDK or later
- Windows 10/11
- Node.js (for web server functionality)

### Build Instructions
1. **Clone Repository**:
   ```bash
   git clone https://github.com/yourusername/minibrowser.git
   cd minibrowser
   ```

2. **Build Application**:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained
   ```

3. **Output Location**: `bin/Release/net6.0-windows/win-x64/publish/`

### Development
- **IDE**: Visual Studio 2022 or Visual Studio Code
- **Framework**: .NET 6.0 Windows Forms
- **Dependencies**: Newtonsoft.Json for settings management

## 📁 File Structure

```
MiniTranslate/
├── MiniTranslate.exe          # Main application
├── miniweb.js                 # Local web server
├── translation-server.html    # Translation interface
├── translator.html            # ChatGPT interface
├── icon.ico                   # Application icon
└── settings.json             # User configuration (in AppData)
```

## ⚙️ Configuration Files

### Settings Location
- **Path**: `%APPDATA%\MiniTranslate\settings.json`
- **Format**: JSON configuration file
- **Backup**: Automatically created on first run

### Log Files
- **Location**: `minitranslate.log` (in application directory)
- **Content**: Server startup/shutdown, translation attempts, errors
- **Rotation**: Manual cleanup required

## 🔒 Privacy & Data Handling

### Data Processing
- **Clipboard Access**: Only reads clipboard content when hotkey is pressed
- **No Storage**: Translation text is not stored locally or remotely
- **API Usage**: Only sends text to translation services when actively translating
- **Session Isolation**: Each translation uses a clean browser session

### Security Features
- **Local Processing**: Language detection runs locally
- **Secure APIs**: HTTPS connections to translation services
- **Token Management**: Secure storage of API keys in local settings
- **No Telemetry**: Application doesn't collect usage statistics

## 🐛 Troubleshooting

### Common Issues
- **Hotkey Not Working**: Check for conflicts with other applications
- **Translation Server Errors**: Verify server URL and authentication tokens
- **Browser Issues**: Ensure Chrome/Edge is installed and accessible
- **API Key Problems**: Validate ChatGPT API key and billing status

### Debug Information
- **Log Files**: Check `minitranslate.log` for detailed error information
- **Settings**: Verify configuration in `%APPDATA%\MiniTranslate\settings.json`
- **Server Status**: Use `curl http://localhost:12345` to test local server

## 📄 License

This project is provided as-is for educational and personal use purposes. 

## 🤝 Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## 📞 Support

For support, please:
1. Check the troubleshooting section above
2. Review the detailed setup guides in the documentation
3. Open an issue on GitHub with detailed error information

---

**MiniTranslate** - Making translation effortless, one hotkey at a time! 🚀
