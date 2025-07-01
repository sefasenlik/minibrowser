# Translation Server Setup Guide (Developer)

Technical guide for setting up a custom translation server to work with MiniTranslate's "Translation Server" option.

## Prerequisites

- Server/VPS outside of Russia (or any region where ChatGPT is blocked)
- Node.js 16+ installed on the server
- ChatGPT API key
- Domain name with SSL certificate (for production)

**Note**: Node.js on the server can be installed through package managers (apt, yum, etc.) or Docker.

## Server Deployment

### Step 1: Install Node.js on Server

```bash
# Ubuntu/Debian
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt-get install -y nodejs

# CentOS/RHEL
curl -fsSL https://rpm.nodesource.com/setup_18.x | sudo bash -
sudo yum install -y nodejs

# Or using Docker (recommended)
docker pull node:18-alpine
```

### Step 2: Upload Server Files

```bash
# Create server directory
mkdir -p /var/www/translation-api
cd /var/www/translation-api

# Upload server files from the 'server' folder
```

### Step 3: Install Dependencies

```bash
npm install
```

### Step 3: Configure Environment

```bash
cp env.example .env
```

Edit `.env` file:
```env
# Server Configuration
PORT=3333

# ChatGPT API Key (server-side)
OPENAI_API_KEY=sk-your-api-key-here

# Default server URL
DEFAULT_SERVER_URL=https://api.sefa.name.tr

# Rate limiting
RATE_LIMIT_WINDOW_MS=900000
RATE_LIMIT_MAX_REQUESTS=100

# CORS settings
CORS_ORIGIN=*
```

### Step 4: Start Server

#### Option A: Docker (Recommended)
```bash
cd server
docker-compose up -d
```

#### Option B: PM2
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

#### Option C: Manual
```bash
npm start
```

## API Endpoints

### Health Check
```
GET /health
```

### Translation
```
POST /translate
Content-Type: application/json
Authorization: Bearer YOUR_TOKEN

{
  "text": "Hello world",
  "sourceLang": "en",
  "targetLang": "ru"
}
```

Response:
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

## Security Configuration

### HTTPS Setup
```bash
# Install Certbot
sudo apt-get install certbot

# Get SSL certificate
sudo certbot certonly --standalone -d your-domain.com

# Configure nginx with SSL
```

### Firewall Configuration
```bash
# Allow only necessary ports
sudo ufw allow 22
sudo ufw allow 80
sudo ufw allow 443
sudo ufw enable
```

### Rate Limiting
Configure in `.env`:
```env
RATE_LIMIT_WINDOW_MS=900000  # 15 minutes
RATE_LIMIT_MAX_REQUESTS=100  # 100 requests per window
```

## Monitoring

### PM2 Monitoring
```bash
pm2 monit
pm2 logs translation-api
pm2 status
```

### Health Check
```bash
curl https://your-domain.com/health
```

## Testing

### Test Translation
```bash
curl -X POST https://your-domain.com/translate \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d '{
    "text": "Hello world",
    "sourceLang": "en",
    "targetLang": "ru"
  }'
```

### Test Languages
```bash
curl https://your-domain.com/languages
```

## MiniTranslate Integration

After server deployment, configure MiniTranslate:

1. Set `PreferredTranslator` to `TranslatorType.TranslationServer`
2. Set `TranslationServerUrl` to your server URL
3. Set `TranslationServerToken` to your authentication token

## Troubleshooting

### Server Won't Start
- Check PORT availability
- Verify Node.js version (16+)
- Check `.env` configuration
- Review error logs

### API Key Issues
- Verify API key format (starts with `sk-`)
- Check OpenAI account credits
- Ensure API key has translation permissions

### CORS Errors
- Verify `CORS_ORIGIN` setting
- Check client domain configuration
- Test with different origins

### Rate Limiting
- Monitor request frequency
- Adjust rate limit settings
- Check OpenAI API limits 