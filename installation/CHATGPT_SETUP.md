# ChatGPT Translator Setup Guide

## Overview
Your MiniTranslate application now supports ChatGPT as a translation service, providing higher quality translations compared to Yandex and Google Translate.

## Setup Instructions

### 1. Get a ChatGPT API Key
1. Go to [OpenAI Platform](https://platform.openai.com/api-keys)
2. Sign in or create an account
3. Click "Create new secret key"
4. Copy the API key (it starts with `sk-`)

### 2. Configure the Translator
1. Right-click the MiniTranslate tray icon
2. Go to **Translator** → **ChatGPT Translator**
3. The application will now use ChatGPT for translations

### 3. Set Up API Key in the Translator
1. When you first use ChatGPT translator, a web page will open
2. Enter your ChatGPT API key in the "API Key Setup" section
3. Click "Save API Key" - it will be stored locally in your browser
4. The translator is now ready to use!

## How to Use

### Method 1: Hotkey (Recommended)
1. Copy text to clipboard
2. Press your configured hotkey (default: Ctrl+Q)
3. The ChatGPT translator will open with your text pre-loaded
4. Translation will start automatically

### Method 2: Tray Menu
1. Copy text to clipboard
2. Right-click the tray icon
3. Click "Open/Translate"

## Features

- **High-quality translations** powered by ChatGPT
- **Supports 35+ languages** including English, Russian, Spanish, French, German, and many more
- **Automatic language detection** from URL parameters
- **Clean, modern interface** optimized for translation workflow
- **Local API key storage** - your key stays on your computer
- **App mode support** - opens in a clean browser window

## Language Support
The ChatGPT translator supports all the same languages as your MiniTranslate application:
- English, Russian, Spanish, French, German, Italian, Portuguese
- Chinese, Japanese, Korean, Arabic, Hindi, Turkish
- Polish, Dutch, Swedish, Danish, Norwegian, Finnish
- Czech, Ukrainian, Bulgarian, Croatian, Slovak, Slovenian
- Estonian, Latvian, Lithuanian, Hungarian, Romanian
- Greek, Hebrew, Thai, Vietnamese, Indonesian, Malay

## Troubleshooting

### "ChatGPT translator file not found"
- Ensure `translator.html` is in the same directory as your MiniTranslate.exe
- The file should be in the application's startup directory

### "Translation failed" errors
- Check that your API key is correct and starts with `sk-`
- Ensure you have sufficient credits in your OpenAI account
- Verify your internet connection

### API Key not saving
- Make sure you're using a modern browser (Chrome, Edge, Firefox)
- Check that cookies/local storage is enabled in your browser

## Cost Information
- ChatGPT API usage is charged per token (word/character)
- Translation costs are typically very low (a few cents per translation)
- You can monitor usage at [OpenAI Platform](https://platform.openai.com/usage)

## Security Notes
- Your API key is stored locally in your browser's localStorage
- The key is never sent to any server except OpenAI's API
- You can delete the key from the translator interface at any time

## Support
If you encounter any issues:
1. Check that both `translator.html` and your application are in the same directory
2. Verify your API key is valid and has sufficient credits
3. Try refreshing the translator page and re-entering your API key 