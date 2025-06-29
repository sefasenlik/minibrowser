# ChatGPT Translation Options Setup Guide

This guide explains the two ChatGPT translation options available in MiniTranslate:

1. **ChatGPT Translator** - Local HTML file (original method)
2. **Translation Server** - Remote API server (bypasses regional blocks)

## Overview

MiniTranslate now supports two different ChatGPT translation methods:

### Option 1: ChatGPT Translator (Local HTML)
- Uses the local `translator.html` file
- Opens in a browser window
- Requires ChatGPT API key in settings
- **Does NOT work in Russia** (ChatGPT is blocked)

### Option 2: Translation Server (Remote API)
- Uses a remote server to handle ChatGPT API calls
- Shows results in a native Windows dialog
- Can use server-side API key (more secure)
- **Works in Russia** and other blocked regions

## Option 1: ChatGPT Translator Setup (Local HTML)

### Prerequisites
- ChatGPT API key from OpenAI
- `translator.html` file in the same directory as MiniTranslate.exe

### Configuration
1. Open MiniTranslate Settings
2. Select "ChatGPT Translator" as preferred translator
3. Enter your ChatGPT API key
4. The `translator.html` file should be in the same folder as the application

### Usage
- Copy text to clipboard
- Press hotkey (Ctrl+Q by default)
- Browser opens with translation interface

## Option 2: Translation Server Setup (Remote API)

### Prerequisites
- A server/VPS outside of Russia (or any region where ChatGPT is not blocked)
- Node.js 16+ installed on the server
- ChatGPT API key (can be stored on server)

### Step 1: Deploy the Translation API Server

#### Option A: Manual Deployment (Recommended)

1. **Upload server files to your server:**
   ```bash
   # On your server, create a directory
   mkdir -p /var/www/translation-api
   cd /var/www/translation-api
   
   # Upload the server files from the 'server' folder
   ```

2. **Install dependencies:**
   ```bash
   npm install
   ```

3. **Configure environment:**
   ```bash
   cp env.example .env
   # Edit .env with your settings
   ```

4. **Start with PM2:**
   ```bash
   # Install PM2 globally
   npm install -g pm2
   
   # Start the server
   pm2 start server.js --name "translation-api"
   
   # Save PM2 configuration
   pm2 save
   
   # Set up PM2 to start on boot
   pm2 startup
   ```

#### Option B: Docker Deployment

1. **Build and run with Docker Compose:**
   ```bash
   cd server
   docker-compose up -d
   ```

2. **Or build manually:**
   ```bash
   docker build -t translation-api .
   docker run -d -p 3333:3333 --name chatgpt-translation-api translation-api
   ```

#### Option C: Cloud Platforms

##### Railway
1. Connect your GitHub repository
2. Set environment variables in Railway dashboard
3. Deploy automatically

##### Heroku
```bash
heroku create your-translation-api
git push heroku main
heroku config:set PORT=3333
```

### Step 2: Configure MiniTranslate for Translation Server

1. **Update your MiniTranslate app** with the new code provided
2. **Build the application** using Visual Studio or your preferred method
3. **Configure the API settings:**
   - Open MiniTranslate
   - Right-click the tray icon → Settings
   - Select "Translation Server" as the preferred translator
   - Enter your server URL (e.g., `https://your-server.com:3333`)

### Step 3: Test the Integration

1. **Test the server:**
   ```bash
   curl https://your-server.com:3333/health
   ```

2. **Test translation:**
   ```bash
   curl -X POST https://your-server.com:3333/translate \
     -H "Content-Type: application/json" \
     -d '{
       "text": "Hello world",
       "sourceLang": "en",
       "targetLang": "ru"
     }'
   ```

3. **Test from MiniTranslate:**
   - Copy some text to clipboard
   - Press your hotkey (Ctrl+Q by default)
   - The translation should appear in a popup window

## Server Configuration

### Environment Variables

Create a `.env` file in your server directory:

```env
# Server Configuration
PORT=3333

# ChatGPT API Key (server-side, more secure)
OPENAI_API_KEY=sk-your-api-key-here

# Optional: Rate limiting
RATE_LIMIT_WINDOW_MS=900000
RATE_LIMIT_MAX_REQUESTS=100

# Optional: CORS settings
CORS_ORIGIN=*
```

### Security Considerations

1. **Use HTTPS** in production
2. **Set up a firewall** to restrict access
3. **Monitor API usage** and costs
4. **Set up logging** for debugging
5. **Use environment variables** for sensitive data

## API Endpoints

### Health Check
```
GET /health
```
Returns server status and uptime information.

### Translation
```
POST /translate
Content-Type: application/json

{
  "text": "Hello world",
  "sourceLang": "en",
  "targetLang": "ru"
}
```

**Note:** The API key can be provided in the request or stored server-side in the `.env` file.

### Get Supported Languages
```
GET /languages
```
Returns list of all supported languages.

## Comparison: ChatGPT Translator vs Translation Server

| Feature | ChatGPT Translator | Translation Server |
|---------|-------------------|-------------------|
| **Regional Blocking** | ❌ Blocked in Russia | ✅ Works everywhere |
| **API Key Security** | ⚠️ Exposed in browser | ✅ Server-side only |
| **User Experience** | ⚠️ Browser window | ✅ Native dialog |
| **Setup Complexity** | ✅ Simple | ⚠️ Requires server |
| **Cost** | ✅ Direct API calls | ⚠️ Server hosting + API |
| **Reliability** | ⚠️ Depends on browser | ✅ More reliable |

## Troubleshooting

### ChatGPT Translator Issues

1. **HTML file not found:**
   - Ensure `translator.html` is in the same directory as MiniTranslate.exe
   - Check file permissions

2. **API key errors:**
   - Verify ChatGPT API key format (must start with `sk-`)
   - Check API key validity and balance

### Translation Server Issues

1. **"Translation failed: Translation failed" error:**
   
   **Step 1: Check server logs**
   ```bash
   # View Docker logs
   docker logs chatgpt-translation-api
   
   # Or if using PM2
   pm2 logs translation-api
   ```

   **Step 2: Test server configuration**
   ```bash
   cd server
   node test-server.js
   ```

   **Step 3: Test API endpoints manually**
   ```bash
   # Test health endpoint
   curl http://your-server:3333/health
   
   # Test translation endpoint
   curl -X POST http://your-server:3333/translate \
     -H "Content-Type: application/json" \
     -d '{"text": "Hello", "sourceLang": "en", "targetLang": "ru"}'
   ```

   **Step 4: Check .env file**
   - Ensure `.env` file exists in server directory
   - Verify `OPENAI_API_KEY=sk-your-actual-key` is set
   - Check file permissions

   **Step 5: Check MiniTranslate settings**
   - Verify Translation Server URL is correct (e.g., `http://your-server:3333`)
   - Ensure "Translation Server" is selected as preferred translator
   - Check Windows console output for debug information

2. **CORS errors:**
   - Check CORS_ORIGIN setting
   - Ensure server allows your domain

3. **Server not starting:**
   - Check PORT availability
   - Verify Node.js version
   - Check logs: `pm2 logs translation-api`

4. **Translation failures:**
   - Check OpenAI API limits
   - Verify text length (max 1000 tokens)
   - Check server logs for errors

### Debug Mode

Enable debug logging:
```bash
DEBUG=* npm start
```

### Monitoring

```bash
# View logs
pm2 logs translation-api

# Monitor processes
pm2 monit

# Check status
pm2 status
```

## Cost Considerations

- **ChatGPT API costs:** ~$0.002 per 1K tokens
- **Server hosting:** $5-20/month depending on provider (only for Translation Server)
- **Bandwidth:** Minimal for translation requests

## Recommendation

- **For users in Russia or blocked regions:** Use Translation Server
- **For users in unrestricted regions:** Either option works, but Translation Server provides better security
- **For simplicity:** Use ChatGPT Translator if you don't want to set up a server

## Support

If you encounter issues:

1. Check the server logs (for Translation Server)
2. Verify your API key and server URL
3. Test the API endpoints manually
4. Ensure your server is accessible from your location 