using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MiniTranslate
{
    public enum BrowserType
    {
        Chrome,
        Edge,
        Default
    }

    public enum TranslatorType
    {
        Yandex,
        Google,
        ChatGPT,
        TranslationServer
    }

    public class AppSettings
    {
        public int HotkeyModifiers { get; set; } = 0x0002; // MOD_CONTROL
        public int HotkeyKey { get; set; } = (int)Keys.Q;
        public string SourceLanguage { get; set; } = "en";
        public string TargetLanguage { get; set; } = "ru";
        public TranslatorType PreferredTranslator { get; set; } = TranslatorType.TranslationServer;
        public int WindowWidth { get; set; } = 850;
        public int WindowHeight { get; set; } = 600;
        public BrowserType PreferredBrowser { get; set; } = BrowserType.Chrome;
        public string ChatGptApiKey { get; set; } = string.Empty;
        public string ChatGptApiServerUrl { get; set; } = "http://localhost:3000";
        public string TranslationServerUrl { get; set; } = "https://api.sefa.name.tr";
        public bool AutoSwitchLanguages { get; set; } = true;
        public string TranslationServerToken { get; set; } = string.Empty;

        // Legacy properties for backward compatibility
        [JsonIgnore]
        public string WebsiteUrl { get; set; } = ""; // Will be ignored
        [JsonIgnore]
        public bool UseTranslateMode { get; set; } = true; // Will be ignored

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "MiniTranslate",
            "settings.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var json = File.ReadAllText(SettingsPath);
                    return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load settings: {ex.Message}. Using defaults.",
                    "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                var directory = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(SettingsPath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save settings: {ex.Message}",
                    "MiniTranslate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 