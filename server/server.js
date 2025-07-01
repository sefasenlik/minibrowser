const express = require('express');
const cors = require('cors');
const fetch = require('node-fetch');
const fs = require('fs');
const path = require('path');
require('dotenv').config();

const app = express();
const PORT = process.env.PORT || 3000;

// Middleware
app.use(cors({
    origin: '*', // Allow all origins for development
    methods: ['GET', 'POST', 'OPTIONS'],
    allowedHeaders: ['Content-Type', 'Authorization'],
    credentials: false
}));
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Health check endpoint
app.get('/health', (req, res) => {
    res.json({ 
        status: 'OK', 
        message: 'ChatGPT Translation API is running',
        timestamp: new Date().toISOString()
    });
});

// Language mapping (same as in the HTML)
const languages = {
    'en': 'English', 'ru': 'Русский', 'es': 'Español', 'fr': 'Français', 'de': 'Deutsch', 
    'it': 'Italiano', 'pt': 'Português', 'zh': '中文', 'ja': '日本語', 'ko': '한국어', 
    'ar': 'العربية', 'hi': 'हिन्दी', 'tr': 'Türkçe', 'pl': 'Polski', 'nl': 'Nederlands', 
    'sv': 'Svenska', 'da': 'Dansk', 'no': 'Norsk', 'fi': 'Suomi', 'cs': 'Čeština', 
    'uk': 'Українська', 'bg': 'Български', 'hr': 'Hrvatski', 'sk': 'Slovenčina', 
    'sl': 'Slovenščina', 'et': 'Eesti', 'lv': 'Latviešu', 'lt': 'Lietuvių', 'hu': 'Magyar', 
    'ro': 'Română', 'el': 'Ελληνικά', 'he': 'עברית', 'th': 'ไทย', 'vi': 'Tiếng Việt', 
    'id': 'Indonesia', 'ms': 'Melayu'
};

// --- User Management ---
const USERS_FILE = path.join(__dirname, 'users.json');
let users = [];

function loadUsers() {
    try {
        users = JSON.parse(fs.readFileSync(USERS_FILE, 'utf8'));
    } catch (e) {
        users = [];
    }
}

function saveUsers() {
    fs.writeFileSync(USERS_FILE, JSON.stringify(users, null, 2), 'utf8');
}

function findUserByToken(token) {
    return users.find(u => u.token === token);
}

function addUser({ name, surname, email, token }) {
    if (users.some(u => u.token === token || u.email === email)) return false;
    users.push({ name, surname, email, token, usage: 0 });
    saveUsers();
    return true;
}

function incrementUserUsage(token, charCount) {
    const user = findUserByToken(token);
    if (user) {
        user.usage = (user.usage || 0) + charCount;
        saveUsers();
    }
}

loadUsers();

// --- Admin Auth Middleware ---
const ADMIN_PASSWORD = process.env.ADMIN_PASSWORD || 'admin123';
function requireAdminAuth(req, res, next) {
    const auth = req.headers['authorization'] || req.query.admin;
    if (auth === ADMIN_PASSWORD) return next();
    res.status(401).send('Unauthorized');
}

// --- Admin Panel Endpoints ---
app.get('/admin', (req, res) => {
    res.sendFile(path.join(__dirname, 'admin.html'));
});

app.get('/admin/users', requireAdminAuth, (req, res) => {
    res.json(users);
});

app.post('/admin/adduser', requireAdminAuth, (req, res) => {
    const { name, surname, email, token } = req.body;
    if (!name || !surname || !email || !token) {
        return res.status(400).json({ error: 'Missing fields' });
    }
    if (addUser({ name, surname, email, token })) {
        res.json({ success: true });
    } else {
        res.status(400).json({ error: 'User already exists' });
    }
});

// Translation endpoint
app.post('/translate', async (req, res) => {
    try {
        const { text, sourceLang, targetLang, apiKey, token } = req.body;
        if (!token || !findUserByToken(token)) {
            return res.status(401).json({ error: 'Invalid or missing user token' });
        }
        // Validation
        if (!text || !sourceLang || !targetLang) {
            return res.status(400).json({
                error: 'Missing required parameters: text, sourceLang, targetLang'
            });
        }

        // Use provided API key or server-side default
        const finalApiKey = apiKey || process.env.OPENAI_API_KEY;
        
        if (!finalApiKey) {
            return res.status(400).json({
                error: 'No API key provided and no server-side API key configured'
            });
        }

        if (!finalApiKey.startsWith('sk-')) {
            return res.status(400).json({
                error: 'Invalid API key format. Must start with sk-'
            });
        }

        // Get language names
        const sourceLanguageName = languages[sourceLang] || sourceLang;
        const targetLanguageName = languages[targetLang] || targetLang;

        // Create prompt
        const prompt = `Translate the following text from ${sourceLanguageName} to ${targetLanguageName}. 
Provide only the translation without any additional explanations, formatting, or quotation marks:

${text}`;

        // Call ChatGPT API
        const response = await fetch('https://api.openai.com/v1/chat/completions', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${finalApiKey}`
            },
            body: JSON.stringify({
                model: 'gpt-4.1-nano',
                messages: [
                    {
                        role: 'user',
                        content: prompt
                    }
                ],
                max_tokens: 1000,
                temperature: 0.3
            })
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.error?.message || `HTTP ${response.status}: ${response.statusText}`);
        }

        const data = await response.json();
        const translation = data.choices[0]?.message?.content?.trim();

        if (!translation) {
            throw new Error('No translation received from ChatGPT');
        }

        // Track usage
        incrementUserUsage(token, text.length);

        // Return successful translation
        res.json({
            success: true,
            translation: translation,
            sourceLang: sourceLang,
            targetLang: targetLang,
            originalText: text
        });

    } catch (error) {
        res.status(500).json({
            error: `Translation failed: ${error.message}`,
            success: false
        });
    }
});

// Get supported languages
app.get('/languages', (req, res) => {
    res.json({
        languages: languages,
        count: Object.keys(languages).length
    });
});

// Error handling middleware
app.use((err, req, res, next) => {
    res.status(500).json({
        error: 'Something went wrong!',
        success: false
    });
});

// 404 handler
app.use((req, res) => {
    res.status(404).json({
        error: 'Endpoint not found',
        success: false
    });
});

// Start server
app.listen(PORT, () => {
    console.log(`🚀 ChatGPT Translation API server running on port ${PORT}`);
    console.log(`📡 Health check: http://localhost:${PORT}/health`);
    console.log(`🌐 Translation endpoint: http://localhost:${PORT}/translate`);
    console.log(`📚 Languages endpoint: http://localhost:${PORT}/languages`);
});

module.exports = app; 