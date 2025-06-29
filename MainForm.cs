using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MiniTranslate
{
    public static class LanguageDetector
    {
        // Script detection based on Unicode ranges
        public static string DetectPrimaryScript(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "unknown";

            var scriptCounts = new Dictionary<string, int>
            {
                {"cyrillic", 0},
                {"latin", 0},
                {"arabic", 0},
                {"chinese", 0},
                {"japanese", 0},
                {"korean", 0},
                {"greek", 0},
                {"hebrew", 0},
                {"thai", 0},
                {"devanagari", 0}
            };

            foreach (char c in text)
            {
                // Skip spaces, punctuation, and numbers
                if (char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsDigit(c) || char.IsSymbol(c))
                    continue;

                int code = (int)c;

                // Cyrillic (Russian, Ukrainian, Bulgarian, etc.)
                if ((code >= 0x0400 && code <= 0x04FF) || (code >= 0x0500 && code <= 0x052F))
                    scriptCounts["cyrillic"]++;
                // Latin (English, European languages)
                else if ((code >= 0x0041 && code <= 0x005A) || (code >= 0x0061 && code <= 0x007A) ||
                         (code >= 0x00C0 && code <= 0x024F))
                    scriptCounts["latin"]++;
                // Arabic
                else if ((code >= 0x0600 && code <= 0x06FF) || (code >= 0x0750 && code <= 0x077F))
                    scriptCounts["arabic"]++;
                // Chinese (CJK Unified Ideographs)
                else if ((code >= 0x4E00 && code <= 0x9FFF) || (code >= 0x3400 && code <= 0x4DBF))
                    scriptCounts["chinese"]++;
                // Japanese (Hiragana, Katakana)
                else if ((code >= 0x3040 && code <= 0x309F) || (code >= 0x30A0 && code <= 0x30FF))
                    scriptCounts["japanese"]++;
                // Korean (Hangul)
                else if ((code >= 0xAC00 && code <= 0xD7AF) || (code >= 0x1100 && code <= 0x11FF))
                    scriptCounts["korean"]++;
                // Greek
                else if ((code >= 0x0370 && code <= 0x03FF) || (code >= 0x1F00 && code <= 0x1FFF))
                    scriptCounts["greek"]++;
                // Hebrew
                else if (code >= 0x0590 && code <= 0x05FF)
                    scriptCounts["hebrew"]++;
                // Thai
                else if (code >= 0x0E00 && code <= 0x0E7F)
                    scriptCounts["thai"]++;
                // Devanagari (Hindi)
                else if (code >= 0x0900 && code <= 0x097F)
                    scriptCounts["devanagari"]++;
            }

            // Find the most common script
            var maxScript = scriptCounts.Aggregate((x, y) => x.Value > y.Value ? x : y);
            return maxScript.Value > 0 ? maxScript.Key : "latin"; // Default to latin if no script detected
        }

        public static string GetSuggestedSourceLanguage(string detectedScript, string currentSourceLang, string currentTargetLang)
        {
            var scriptToLanguageMap = new Dictionary<string, string[]>
            {
                {"cyrillic", new[] {"ru", "uk", "bg", "sr", "mk"}},
                {"latin", new[] {"en", "es", "fr", "de", "it", "pt", "pl", "nl", "sv", "da", "no", "fi", "cs", "sk", "sl", "et", "lv", "lt", "hu", "ro", "hr"}},
                {"arabic", new[] {"ar"}},
                {"chinese", new[] {"zh"}},
                {"japanese", new[] {"ja"}},
                {"korean", new[] {"ko"}},
                {"greek", new[] {"el"}},
                {"hebrew", new[] {"he"}},
                {"thai", new[] {"th"}},
                {"devanagari", new[] {"hi"}}
            };

            if (!scriptToLanguageMap.ContainsKey(detectedScript))
                return currentSourceLang;

            var possibleLanguages = scriptToLanguageMap[detectedScript];
            
            // If current source language matches the detected script, keep it
            if (possibleLanguages.Contains(currentSourceLang))
                return currentSourceLang;

            // Otherwise, return the first (most common) language for this script
            return possibleLanguages[0];
        }

        public static (string newSource, string newTarget) GetAutoSwitchedLanguages(string text, string currentSource, string currentTarget)
        {
            var detectedScript = DetectPrimaryScript(text);
            var suggestedSource = GetSuggestedSourceLanguage(detectedScript, currentSource, currentTarget);

            // If the suggested source is different from current source, switch
            if (suggestedSource != currentSource)
            {
                // If the suggested source matches current target, swap them
                if (suggestedSource == currentTarget)
                {
                    return (currentTarget, currentSource);
                }
                // Otherwise, use suggested source and keep current target
                else
                {
                    return (suggestedSource, currentTarget);
                }
            }

            // No change needed
            return (currentSource, currentTarget);
        }
    }

    public static class LanguageProvider
    {
        public static Dictionary<string, string> GetLanguages()
        {
            return new Dictionary<string, string>
            {
                {"en", "English"}, {"ru", "Русский"}, {"es", "Español"}, {"fr", "Français"},
                {"de", "Deutsch"}, {"it", "Italiano"}, {"pt", "Português"}, {"zh", "中文"},
                {"ja", "日本語"}, {"ko", "한국어"}, {"ar", "العربية"}, {"hi", "हिन्दी"},
                {"tr", "Türkçe"}, {"pl", "Polski"}, {"nl", "Nederlands"}, {"sv", "Svenska"},
                {"da", "Dansk"}, {"no", "Norsk"}, {"fi", "Suomi"}, {"cs", "Čeština"},
                {"uk", "Українська"}, {"bg", "Български"}, {"hr", "Hrvatski"}, {"sk", "Slovenčina"},
                {"sl", "Slovenščina"}, {"et", "Eesti"}, {"lv", "Latviešu"}, {"lt", "Lietuvių"},
                {"hu", "Magyar"}, {"ro", "Română"}, {"el", "Ελληνικά"}, {"he", "עברית"},
                {"th", "ไทย"}, {"vi", "Tiếng Việt"}, {"id", "Indonesia"}, {"ms", "Melayu"}
            };
        }
    }

    public partial class MainForm : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private AppSettings settings;
        private int hotKeyId = 1;
        private static readonly HttpClient httpClient = new HttpClient();
        private Process nodeServerProcess;

        // Windows API for global hotkeys
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        // Hotkey modifiers
        private const int MOD_CONTROL = 0x0002;
        private const int MOD_ALT = 0x0001;
        private const int MOD_SHIFT = 0x0004;
        private const int MOD_WIN = 0x0008;

        // Windows message for hotkey
        private const int WM_HOTKEY = 0x0312;

        public MainForm()
        {
            InitializeComponent();
            LoadSettings();
            InitializeTrayIcon();
            RegisterGlobalHotKey();
            UpdateTrayIconText();
            StartNodeServer();
            
            // Hide the form initially
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
            Visible = false;
        }

        private void InitializeComponent()
        {
            this.Text = "MiniTranslate";
            this.Size = new Size(1, 1);
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar = false;
        }

        private void InitializeTrayIcon()
        {
            trayMenu = new ContextMenuStrip();
            trayIcon = new NotifyIcon()
            {
                Text = "MiniTranslate", // Set initial placeholder text
                Icon = LoadCustomIcon(),
                ContextMenuStrip = trayMenu,
                Visible = true
            };

            BuildTrayMenu();
            trayIcon.DoubleClick += OnOpenWebsite;
        }

        private void BuildTrayMenu()
        {
            trayMenu.Items.Clear();

            trayMenu.Items.Add("Open/Translate", null, OnOpenWebsite);
            trayMenu.Items.Add("-");

            // Switch Languages
            trayMenu.Items.Add("Switch Languages", null, OnSwitchLanguages);
            trayMenu.Items.Add("-");

            // Translator Service Menu
            var translatorMenu = new ToolStripMenuItem("Translator");
            var yandexItem = new ToolStripMenuItem("Yandex Translate", null, OnTranslatorChanged) { Tag = TranslatorType.Yandex };
            var googleItem = new ToolStripMenuItem("Google Translate", null, OnTranslatorChanged) { Tag = TranslatorType.Google };
            var chatgptItem = new ToolStripMenuItem("ChatGPT Translator", null, OnTranslatorChanged) { Tag = TranslatorType.ChatGPT };
            var translationServerItem = new ToolStripMenuItem("Translation Server", null, OnTranslatorChanged) { Tag = TranslatorType.TranslationServer };
            yandexItem.Checked = settings.PreferredTranslator == TranslatorType.Yandex;
            googleItem.Checked = settings.PreferredTranslator == TranslatorType.Google;
            chatgptItem.Checked = settings.PreferredTranslator == TranslatorType.ChatGPT;
            translationServerItem.Checked = settings.PreferredTranslator == TranslatorType.TranslationServer;
            translatorMenu.DropDownItems.Add(yandexItem);
            translatorMenu.DropDownItems.Add(googleItem);
            translatorMenu.DropDownItems.Add(chatgptItem);
            translatorMenu.DropDownItems.Add(translationServerItem);
            trayMenu.Items.Add(translatorMenu);

            // Browser Menu
            var browserMenu = new ToolStripMenuItem("Browser");
            var chromeItem = new ToolStripMenuItem("Chrome", null, OnBrowserChanged) { Tag = BrowserType.Chrome };
            var edgeItem = new ToolStripMenuItem("Edge", null, OnBrowserChanged) { Tag = BrowserType.Edge };
            var defaultItem = new ToolStripMenuItem("Default", null, OnBrowserChanged) { Tag = BrowserType.Default };
            chromeItem.Checked = settings.PreferredBrowser == BrowserType.Chrome;
            edgeItem.Checked = settings.PreferredBrowser == BrowserType.Edge;
            defaultItem.Checked = settings.PreferredBrowser == BrowserType.Default;
            browserMenu.DropDownItems.Add(chromeItem);
            browserMenu.DropDownItems.Add(edgeItem);
            browserMenu.DropDownItems.Add(defaultItem);
            trayMenu.Items.Add(browserMenu);
            
            // Language Menus
            var sourceLangMenu = new ToolStripMenuItem("Source Language");
            var targetLangMenu = new ToolStripMenuItem("Target Language");

            var languages = LanguageProvider.GetLanguages();
            foreach (var lang in languages)
            {
                // Add to Source Language Menu
                var sourceItem = new ToolStripMenuItem(lang.Value, null, OnSourceLanguageChanged) { Tag = lang.Key };
                sourceItem.Checked = settings.SourceLanguage == lang.Key;
                sourceLangMenu.DropDownItems.Add(sourceItem);

                // Add to Target Language Menu
                var targetItem = new ToolStripMenuItem(lang.Value, null, OnTargetLanguageChanged) { Tag = lang.Key };
                targetItem.Checked = settings.TargetLanguage == lang.Key;
                targetLangMenu.DropDownItems.Add(targetItem);
            }

            trayMenu.Items.Add(sourceLangMenu);
            trayMenu.Items.Add(targetLangMenu);

            trayMenu.Items.Add("-");
            trayMenu.Items.Add("Settings", null, OnSettings);
            trayMenu.Items.Add("Exit", null, OnExit);
        }

        private Icon LoadCustomIcon()
        {
            // Try to load icon from multiple possible locations
            string[] iconPaths = {
                "icon.ico",  // Current directory first
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "icon.ico"),  // Application directory
                Path.Combine(Environment.CurrentDirectory, "icon.ico"),  // Working directory
                Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? "", "icon.ico")  // Executable directory
            };

            foreach (string iconPath in iconPaths)
            {
                try
                {
                    if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
                    {
                        return new Icon(iconPath);
                    }
                }
                catch
                {
                    // Continue to next path if this one fails
                    continue;
                }
            }
            
            // If no icon file found, create default
            return CreateDefaultIcon();
        }

        private Icon CreateDefaultIcon()
        {
            // Create a simple default icon with "T" for Translator
            Bitmap bitmap = new Bitmap(16, 16);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.DarkBlue, 0, 0, 16, 16);
                g.FillRectangle(Brushes.White, 2, 2, 12, 12);
                g.DrawString("T", new Font("Arial", 8, FontStyle.Bold), Brushes.DarkBlue, 3, 1);
            }
            return Icon.FromHandle(bitmap.GetHicon());
        }

        private void LoadSettings()
        {
            settings = AppSettings.Load();
            // DO NOT update tray icon text here, as the icon may not exist yet.
        }

        private void UpdateTrayIconText()
        {
            if (trayIcon == null) return; // Add a safeguard

            var hotkeyText = GetHotkeyDisplayString();
            string translatorName;
            switch (settings.PreferredTranslator)
            {
                case TranslatorType.Google:
                    translatorName = "Google";
                    break;
                case TranslatorType.ChatGPT:
                    translatorName = "ChatGPT";
                    break;
                case TranslatorType.TranslationServer:
                    translatorName = "Translation Server";
                    break;
                default:
                    translatorName = "Yandex";
                    break;
            }
            trayIcon.Text = $"MiniTranslate - {hotkeyText} to translate clipboard ({settings.SourceLanguage}→{settings.TargetLanguage}) via {translatorName}";
        }

        private string GetHotkeyDisplayString()
        {
            var parts = new List<string>();
            
            if ((settings.HotkeyModifiers & MOD_CONTROL) != 0) parts.Add("Ctrl");
            if ((settings.HotkeyModifiers & MOD_ALT) != 0) parts.Add("Alt");
            if ((settings.HotkeyModifiers & MOD_SHIFT) != 0) parts.Add("Shift");
            if ((settings.HotkeyModifiers & MOD_WIN) != 0) parts.Add("Win");
            
            parts.Add(((Keys)settings.HotkeyKey).ToString());
            
            return string.Join("+", parts);
        }

        private void RegisterGlobalHotKey()
        {
            UnregisterHotKey(this.Handle, hotKeyId);
            
            if (!RegisterHotKey(this.Handle, hotKeyId, settings.HotkeyModifiers, settings.HotkeyKey))
            {
                MessageBox.Show($"Failed to register hotkey {GetHotkeyDisplayString()}. It might be already in use.",
                    "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            // Update tray icon if settings changed
            UpdateTrayIcon();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY && m.WParam.ToInt32() == hotKeyId)
            {
                OpenWebsite();
            }
            base.WndProc(ref m);
        }

        private void OnOpenWebsite(object sender, EventArgs e)
        {
            OpenWebsite();
        }

        private void OpenWebsite()
        {
            try
            {
                // Get text from clipboard for translation
                string clipboardText = GetClipboardText();
                if (string.IsNullOrEmpty(clipboardText))
                {
                    MessageBox.Show("No text found in clipboard to translate.", 
                        "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Auto-switch languages if enabled
                if (settings.AutoSwitchLanguages)
                {
                    var (newSource, newTarget) = LanguageDetector.GetAutoSwitchedLanguages(
                        clipboardText, settings.SourceLanguage, settings.TargetLanguage);
                    
                    if (newSource != settings.SourceLanguage || newTarget != settings.TargetLanguage)
                    {
                        settings.SourceLanguage = newSource;
                        settings.TargetLanguage = newTarget;
                        settings.Save();
                        BuildTrayMenu(); // Update menu to reflect language changes
                        UpdateTrayIconText();
                    }
                }

                string url;
                // Always use the embedded HTTP server for local HTML
                int serverPort = 12345;
                string nodeExe = "node"; // Assumes node is in PATH or bundled in app dir
                string miniwebPath = Path.Combine(Application.StartupPath, "miniweb.js");

                // Check if the server is running (try to connect)
                bool serverRunning = false;
                try
                {
                    using (var client = new System.Net.Sockets.TcpClient())
                    {
                        client.Connect("127.0.0.1", serverPort);
                        serverRunning = true;
                    }
                }
                catch { }

                // If not running, launch it
                if (!serverRunning && File.Exists(miniwebPath))
                {
                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = nodeExe,
                            Arguments = $"\"{miniwebPath}\"",
                            WorkingDirectory = Application.StartupPath,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process.Start(psi);
                        // Wait a moment for the server to start
                        System.Threading.Thread.Sleep(500);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to start embedded server: {ex.Message}", "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (settings.PreferredTranslator == TranslatorType.TranslationServer)
                {
                    url = $"http://localhost:{serverPort}/translation-server.html?sl={settings.SourceLanguage}&tl={settings.TargetLanguage}&text={Uri.EscapeDataString(clipboardText)}&server={Uri.EscapeDataString(settings.TranslationServerUrl)}&token={Uri.EscapeDataString(settings.TranslationServerToken)}";
                }
                else if (settings.PreferredTranslator == TranslatorType.Google)
                {
                    url = $"https://translate.google.com/?sl={settings.SourceLanguage}&tl={settings.TargetLanguage}&text={Uri.EscapeDataString(clipboardText)}&op=translate";
                }
                else if (settings.PreferredTranslator == TranslatorType.ChatGPT)
                {
                    url = $"http://localhost:{serverPort}/translator.html?sl={settings.SourceLanguage}&tl={settings.TargetLanguage}&text={Uri.EscapeDataString(clipboardText)}&apikey={Uri.EscapeDataString(settings.ChatGptApiKey)}";
                }
                else
                {
                    url = $"https://translate.yandex.com/?source_lang={settings.SourceLanguage}&target_lang={settings.TargetLanguage}&text={Uri.EscapeDataString(clipboardText)}";
                }

                // Try to open in app mode (minimal UI) with Chrome or Edge
                if (!TryOpenInAppMode(url))
                {
                    // Fallback to default browser if app mode fails
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open: {ex.Message}",
                    "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetClipboardText()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    return Clipboard.GetText().Trim();
                }
                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private bool TryOpenInAppMode(string url)
        {
            // Generate a temporary user data directory for a clean session
            string tempUserDataDir = Path.Combine(Path.GetTempPath(), "MiniTranslate_" + Guid.NewGuid().ToString("N"));

            switch (settings.PreferredBrowser)
            {
                case BrowserType.Chrome:
                    return TryOpenWithChrome(url, tempUserDataDir);
                    
                case BrowserType.Edge:
                    return TryOpenWithEdge(url, tempUserDataDir);
                    
                case BrowserType.Default:
                    // Default browser doesn't support app mode or custom user directories
                    return false; 
                    
                default:
                    // Fallback to old behavior (try Chrome first, then Edge)
                    return TryOpenWithChrome(url, tempUserDataDir) || TryOpenWithEdge(url, tempUserDataDir);
            }
        }

        private bool TryOpenWithChrome(string url, string userDataDir)
        {
            var sizeArgs = $"--window-size={settings.WindowWidth},{settings.WindowHeight}";
            var userDataArgs = $"--user-data-dir=\"{userDataDir}\"";
            var arguments = $"--app={url} {sizeArgs} {userDataArgs} --disable-extensions";

            // Try Chrome with generic name first
            if (TryStartProcess("chrome.exe", arguments))
                return true;

            // Try Chrome in common installation paths
            string[] chromePaths = {
                @"C:\Program Files\Google\Chrome\Application\chrome.exe",
                @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe",
                Environment.ExpandEnvironmentVariables(@"%LOCALAPPDATA%\Google\Chrome\Application\chrome.exe")
            };

            foreach (var chromePath in chromePaths)
            {
                if (File.Exists(chromePath) && TryStartProcess(chromePath, arguments))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryOpenWithEdge(string url, string userDataDir)
        {
            var sizeArgs = $"--window-size={settings.WindowWidth},{settings.WindowHeight}";
            var userDataArgs = $"--user-data-dir=\"{userDataDir}\"";
            var arguments = $"--app={url} {sizeArgs} {userDataArgs} --disable-extensions";
            
            // Try Edge with generic name first
            if (TryStartProcess("msedge.exe", arguments))
                return true;

            // Try Edge in common installation paths
            string[] edgePaths = {
                @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
                @"C:\Program Files\Microsoft\Edge\Application\msedge.exe"
            };

            foreach (var edgePath in edgePaths)
            {
                if (File.Exists(edgePath) && TryStartProcess(edgePath, arguments))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryStartProcess(string fileName, string arguments)
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(processStartInfo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateTrayIcon()
        {
            if (trayIcon != null)
            {
                var newIcon = LoadCustomIcon();
                if (trayIcon.Icon != newIcon)
                {
                    trayIcon.Icon?.Dispose();
                    trayIcon.Icon = newIcon;
                }
            }
        }

        private void OnTranslatorChanged(object sender, EventArgs e)
        {
            var selectedItem = sender as ToolStripMenuItem;
            if (selectedItem?.Tag is TranslatorType selectedTranslator)
            {
                settings.PreferredTranslator = selectedTranslator;
                settings.Save();
                UpdateTrayIconText();
                BuildTrayMenu(); // Rebuild menu to update checkmarks
            }
        }

        private void OnBrowserChanged(object sender, EventArgs e)
        {
            var selectedItem = sender as ToolStripMenuItem;
            if (selectedItem?.Tag is BrowserType selectedBrowser)
            {
                settings.PreferredBrowser = selectedBrowser;
                settings.Save();
                BuildTrayMenu(); // Rebuild menu to update checkmarks
            }
        }

        private void OnSourceLanguageChanged(object sender, EventArgs e)
        {
            var selectedItem = sender as ToolStripMenuItem;
            if (selectedItem?.Tag is string langCode)
            {
                settings.SourceLanguage = langCode;
                settings.Save();
                UpdateTrayIconText();
                BuildTrayMenu();
            }
        }

        private void OnTargetLanguageChanged(object sender, EventArgs e)
        {
            var selectedItem = sender as ToolStripMenuItem;
            if (selectedItem?.Tag is string langCode)
            {
                settings.TargetLanguage = langCode;
                settings.Save();
                UpdateTrayIconText();
                BuildTrayMenu();
            }
        }

        private void OnSettings(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(settings))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    settings = settingsForm.Settings;
                    settings.Save();
                    RegisterGlobalHotKey();
                    UpdateTrayIconText();
                    UpdateTrayIcon();
                    BuildTrayMenu(); // Rebuild menu to reflect changes
                }
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            KillNodeServer();
            UnregisterHotKey(this.Handle, hotKeyId);
            trayIcon.Visible = false;
            Application.Exit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                KillNodeServer();
                UnregisterHotKey(this.Handle, hotKeyId);
                trayIcon?.Dispose();
                trayMenu?.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void LogToFile(string message)
        {
            try
            {
                var logPath = Path.Combine(Application.StartupPath, "minitranslate.log");
                var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var logMessage = $"[{timestamp}] {message}";
                
                File.AppendAllText(logPath, logMessage + Environment.NewLine);
                Console.WriteLine(logMessage);
            }
            catch
            {
                // Ignore logging errors
            }
        }

        private void OnSwitchLanguages(object sender, EventArgs e)
        {
            // Swap source and target languages
            var temp = settings.SourceLanguage;
            settings.SourceLanguage = settings.TargetLanguage;
            settings.TargetLanguage = temp;
            settings.Save();
            BuildTrayMenu();
        }

        private void StartNodeServer()
        {
            try
            {
                // Kill any existing node processes on port 12345
                KillExistingNodeProcesses();
                
                // Start the miniweb.js server
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "node",
                    Arguments = "miniweb.js",
                    WorkingDirectory = Application.StartupPath,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                nodeServerProcess = new Process
                {
                    StartInfo = processStartInfo,
                    EnableRaisingEvents = true
                };

                // Add event handlers for output
                nodeServerProcess.OutputDataReceived += (sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        LogToFile($"Node.js output: {e.Data}");
                };
                
                nodeServerProcess.ErrorDataReceived += (sender, e) => {
                    if (!string.IsNullOrEmpty(e.Data))
                        LogToFile($"Node.js error: {e.Data}");
                };

                bool started = nodeServerProcess.Start();
                if (started)
                {
                    nodeServerProcess.BeginOutputReadLine();
                    nodeServerProcess.BeginErrorReadLine();
                    LogToFile($"Node.js server started with PID: {nodeServerProcess.Id}");
                    
                    // Wait a moment and check if server is actually running
                    System.Threading.Thread.Sleep(1000);
                    if (IsServerRunning())
                    {
                        LogToFile("Node.js server is running and responding on port 12345");
                    }
                    else
                    {
                        LogToFile("Warning: Node.js process started but server is not responding on port 12345");
                    }
                }
                else
                {
                    LogToFile("Failed to start Node.js server - process.Start() returned false");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Failed to start Node.js server: {ex.Message}");
                LogToFile($"Exception details: {ex}");
            }
        }

        private void KillExistingNodeProcesses()
        {
            try
            {
                // Kill any node processes that might be using port 12345
                var processes = Process.GetProcessesByName("node");
                foreach (var process in processes)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(3000); // Wait up to 3 seconds
                        LogToFile($"Killed existing node process with PID: {process.Id}");
                    }
                    catch (Exception ex)
                    {
                        LogToFile($"Failed to kill node process {process.Id}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Error killing existing node processes: {ex.Message}");
            }
        }

        private void KillNodeServer()
        {
            try
            {
                if (nodeServerProcess != null && !nodeServerProcess.HasExited)
                {
                    nodeServerProcess.Kill();
                    nodeServerProcess.WaitForExit(3000); // Wait up to 3 seconds
                    LogToFile("Node.js server killed");
                }
            }
            catch (Exception ex)
            {
                LogToFile($"Failed to kill Node.js server: {ex.Message}");
            }
        }

        private bool IsServerRunning()
        {
            try
            {
                using (var client = new System.Net.Sockets.TcpClient())
                {
                    var result = client.BeginConnect("127.0.0.1", 12345, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));
                    client.EndConnect(result);
                    return success;
                }
            }
            catch
            {
                return false;
            }
        }
    }
} 