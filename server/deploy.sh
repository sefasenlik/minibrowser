#!/bin/bash

# ChatGPT Translation API Server Deployment Script
# This script helps deploy the translation API to a Linux server

echo "🚀 ChatGPT Translation API Server Deployment"
echo "============================================="

# Check if Node.js is installed
if ! command -v node &> /dev/null; then
    echo "❌ Node.js is not installed. Installing Node.js 18..."
    curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
    sudo apt-get install -y nodejs
else
    echo "✅ Node.js is already installed: $(node --version)"
fi

# Check if npm is installed
if ! command -v npm &> /dev/null; then
    echo "❌ npm is not installed. Please install Node.js with npm."
    exit 1
else
    echo "✅ npm is already installed: $(npm --version)"
fi

# Install PM2 globally if not installed
if ! command -v pm2 &> /dev/null; then
    echo "📦 Installing PM2 for process management..."
    sudo npm install -g pm2
else
    echo "✅ PM2 is already installed: $(pm2 --version)"
fi

# Install dependencies
echo "📦 Installing project dependencies..."
npm install

# Create .env file if it doesn't exist
if [ ! -f .env ]; then
    echo "📝 Creating .env file from template..."
    cp env.example .env
    echo "⚠️  Please edit .env file with your configuration before starting the server"
fi

# Set up PM2 ecosystem file
echo "📝 Creating PM2 ecosystem file..."
cat > ecosystem.config.js << EOF
module.exports = {
  apps: [{
    name: 'translation-api',
    script: 'server.js',
    instances: 1,
    autorestart: true,
    watch: false,
    max_memory_restart: '1G',
    env: {
      NODE_ENV: 'production',
      PORT: 3000
    }
  }]
};
EOF

echo ""
echo "✅ Deployment completed!"
echo ""
echo "📋 Next steps:"
echo "1. Edit .env file with your configuration"
echo "2. Start the server: pm2 start ecosystem.config.js"
echo "3. Save PM2 configuration: pm2 save"
echo "4. Set up PM2 to start on boot: pm2 startup"
echo ""
echo "🔧 Useful commands:"
echo "  pm2 start translation-api     # Start the server"
echo "  pm2 stop translation-api      # Stop the server"
echo "  pm2 restart translation-api   # Restart the server"
echo "  pm2 logs translation-api      # View logs"
echo "  pm2 monit                     # Monitor processes"
echo ""
echo "🌐 Server will be available at: http://your-server-ip:3000"
echo "📡 Health check: http://your-server-ip:3000/health" 