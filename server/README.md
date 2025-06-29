# ChatGPT Translation API Server

A Node.js/Express server that provides ChatGPT translation services via REST API. This server can be deployed to bypass regional restrictions and provide translation services to your MiniTranslate application.

## Features

- **RESTful API** for ChatGPT translations
- **CORS enabled** for cross-origin requests
- **Input validation** and error handling
- **Health check endpoint** for monitoring
- **Language support** for 35+ languages
- **Rate limiting** (optional)
- **Environment configuration**

## Quick Start

### Prerequisites

- Node.js 16+ 
- npm or yarn
- ChatGPT API key

### Installation

1. **Clone or download** the server files to your server
2. **Install dependencies:**
   ```bash
   cd server
   npm install
   ```

3. **Configure environment:**
   ```bash
   cp env.example .env
   # Edit .env with your settings
   ```

4. **Start the server:**
   ```bash
   npm start
   ```

### Development Mode

```bash
npm run dev
```

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
  "targetLang": "ru",
  "apiKey": "sk-your-api-key"
}
```

**Response:**
```json
{
  "success": true,
  "translation": "Привет мир",
  "sourceLang": "en",
  "targetLang": "ru",
  "originalText": "Hello world"
}
```

### Get Supported Languages
```
GET /languages
```
Returns list of all supported languages.

## Deployment Options

### 1. VPS/Cloud Server (Recommended)

**Requirements:**
- Ubuntu/CentOS server
- Node.js 16+
- PM2 (for process management)

**Deployment steps:**
```bash
# Install Node.js
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# Install PM2
npm install -g pm2

# Clone/upload your server files
cd /var/www/translation-api
npm install

# Start with PM2
pm2 start server.js --name "translation-api"
pm2 startup
pm2 save
```

### 2. Railway/Heroku

**Railway:**
1. Connect your GitHub repository
2. Set environment variables
3. Deploy automatically

**Heroku:**
```bash
heroku create your-translation-api
git push heroku main
heroku config:set PORT=3000
```

### 3. Docker

Create a `Dockerfile`:
```dockerfile
FROM node:18-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 3000
CMD ["npm", "start"]
```

Build and run:
```bash
docker build -t translation-api .
docker run -p 3000:3000 translation-api
```

## Configuration

### Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `PORT` | Server port | 3000 |
| `OPENAI_API_KEY` | Default API key (optional) | - |
| `RATE_LIMIT_WINDOW_MS` | Rate limit window | 900000 (15min) |
| `RATE_LIMIT_MAX_REQUESTS` | Max requests per window | 100 |
| `CORS_ORIGIN` | CORS origin | * |

### Security Considerations

1. **Use HTTPS** in production
2. **Set up rate limiting** for public APIs
3. **Validate API keys** on your server
4. **Monitor usage** and costs
5. **Set up logging** for debugging

## Integration with MiniTranslate

After deploying your server, update your MiniTranslate application to use the API instead of the local HTML file.

The server URL will be: `https://your-server.com:3000/translate`

## Monitoring

### Health Check
```bash
curl https://your-server.com:3000/health
```

### Logs (with PM2)
```bash
pm2 logs translation-api
pm2 monit
```

## Troubleshooting

### Common Issues

1. **CORS errors**: Check CORS_ORIGIN setting
2. **API key errors**: Verify ChatGPT API key format
3. **Rate limiting**: Check OpenAI API limits
4. **Server not starting**: Check PORT availability

### Debug Mode

Enable debug logging:
```bash
DEBUG=* npm start
```

## License

MIT License - feel free to modify and deploy as needed. 