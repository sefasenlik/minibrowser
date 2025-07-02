# MiniTranslate - Advanced Translation Utility

A powerful Windows utility that provides instant, high-quality translations using global keyboard shortcuts. MiniTranslate rivals DeepL in translation quality while offering unique features like regional accessibility and confidential server-side processing.

<img src="https://gcdnb.pbrd.co/images/38DcE19tdp5j.png" width="450" />

## 🌟 Key Features

### 🔥 Core Functionality
- **Global Hotkey Translation**: Works system-wide with customizable shortcuts (default: `Ctrl+Q`)
- **Multiple Translation Services**: Choose from Google Translate, Yandex Translate, ChatGPT, or confidential Translation Server
- **Intelligent Language Detection**: Automatically detects text script and suggests optimal language pairs
- **Auto-Switching Languages**: Smart language switching based on clipboard content analysis
- **Regional Accessibility**: Translation Server option works in all regions, including Russia where ChatGPT is blocked

### 🎯 Translation Services
- **Google Translate**: Fast, reliable translations with extensive language support (100+ languages)
- **Yandex Translate**: High-quality translations with excellent Russian language support (90+ languages)
- **ChatGPT Translator**: AI-powered translations with superior quality and context understanding (35+ languages)
- **Translation Server**: Confidential server-side processing with token-based authentication, rivals DeepL quality
- **Context Support**: Provide translation context and requirements (e.g., male/female speaker, preferred terms) for more accurate translations

### 🌐 Browser Integration
- **Multiple Browser Support**: Chrome, Edge, or default browser
- **App Mode**: Clean, minimal browser windows without toolbars
- **Customizable Window Size**: Configurable translation window dimensions
- **Isolated Sessions**: Temporary user data directories for clean browsing

### 🛡️ Privacy & Security
- **Confidential Processing**: Translation Server option processes text server-side with no data retention
- **Regional Accessibility**: Works in all regions including Russia where ChatGPT is blocked
- **Secure API Integration**: Token-based authentication for confidential translation servers
- **Temporary Data**: Browser sessions use isolated temporary directories
- **No Data Collection**: Application doesn't collect or store user data
- **Local Settings**: All configuration stored locally in user's AppData folder
- **DeepL Alternative**: Translation Server provides comparable quality to DeepL with better regional accessibility

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
- Node.js executable (included in application directory)

### Installation
1. **Download**: Get the `MiniTranslateSetup.exe` installer from the [Releases](../../releases) page
2. **Install**: Run the installer and follow the setup wizard
3. **Launch**: The application will start automatically after installation

### First Translation
1. **Copy Text**: Select text in any application and press `Ctrl+C`
2. **Press Hotkey**: Use your configured hotkey (default: `Ctrl+Q`)
3. **Get Translation**: A translation window opens with your translated text

For detailed installation and usage instructions, see [docs/HOW_TO_USE.md](docs/HOW_TO_USE.md).

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

## 🤖 Translation Services

### ChatGPT Translator
- **High-Quality Translations**: Context-aware AI translations
- **Context Support**: Provide translation context and requirements for more accurate translations
- **Direct API Integration**: Native Windows dialog interface
- **Secure API**: Direct integration with OpenAI's API
- **Language Support**: 35+ major languages

For setup instructions, see [docs/CHATGPT_SETUP.md](docs/CHATGPT_SETUP.md).

### Translation Server (DeepL Alternative)
- **Confidential Processing**: Server-side processing with no data retention
- **Context Support**: Provide translation context and requirements for more accurate translations
- **Regional Accessibility**: Works in all regions including Russia
- **Token Authentication**: Secure access with user tokens
- **DeepL Quality**: Comparable translation quality to DeepL
- **Free Service**: Contact senliksefa@gmail.com for server token

For server setup instructions, see [docs/CHATGPT_API_SETUP.md](docs/CHATGPT_API_SETUP.md).

## 🌍 Language Support

MiniTranslate supports **35+ languages** including:

**European Languages**: English, Russian, Spanish, French, German, Italian, Portuguese, Polish, Dutch, Swedish, Danish, Norwegian, Finnish, Czech, Slovak, Slovenian, Estonian, Latvian, Lithuanian, Hungarian, Romanian, Croatian, Greek

**Asian Languages**: Chinese, Japanese, Korean, Arabic, Hindi, Thai, Vietnamese, Indonesian, Malay

**Other Scripts**: Hebrew

### Intelligent Language Detection
The app automatically detects text scripts (Cyrillic, Latin, Arabic, Chinese, Japanese, Korean, Greek, Hebrew, Thai, Devanagari) and suggests optimal language pairs for translation.

## 🏢 Enterprise Features

### Translation Server Integration
- **Confidential Processing**: Server-side processing with no data retention
- **Regional Accessibility**: Works in all regions including Russia
- **Token Authentication**: Secure access with user tokens
- **DeepL Alternative**: Comparable quality to DeepL with better accessibility
- **Admin Panel**: Web-based user management interface
- **Docker Support**: Easy deployment with Docker containers
- **Load Balancing**: Support for multiple server instances

For server setup instructions, see [docs/CHATGPT_API_SETUP.md](docs/CHATGPT_API_SETUP.md).

## 🔨 Building from Source

### Prerequisites
- .NET 6.0 SDK or later
- Windows 10/11
- Node.js executable (download from https://nodejs.org/download/release/latest/win-x64/node.exe)

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

3. **Create Installer**: Use the provided `.iss` file to create the installer
4. **Output Location**: `bin/Release/net6.0-windows/win-x64/publish/`

### Development
- **IDE**: Visual Studio 2022 or Visual Studio Code
- **Framework**: .NET 6.0 Windows Forms
- **Dependencies**: Newtonsoft.Json for settings management

## 📁 File Structure

```
MiniTranslate/
├── MiniTranslate.exe          # Main application
├── miniweb.js                 # Local web server
├── node.exe                   # Node.js runtime (for local server)
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
- **Confidential Processing**: Translation Server option processes text server-side with no data retention
- **API Usage**: Only sends text to translation services when actively translating
- **Session Isolation**: Each translation uses a clean browser session

### Security Features
- **Local Processing**: Language detection runs locally
- **Secure APIs**: HTTPS connections to translation services
- **Token Management**: Secure storage of API keys in local settings
- **Regional Accessibility**: Translation Server works in all regions including Russia
- **DeepL Alternative**: Comparable quality to DeepL with better regional accessibility
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
4. Contact senliksefa@gmail.com

---

**MiniTranslate** - Making translation effortless, one hotkey at a time! 🚀
