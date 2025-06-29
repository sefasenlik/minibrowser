using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Windows.Forms;

namespace MiniTranslate.Services
{
    public class ChatGPTTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ChatGPTTranslationService(string apiBaseUrl = "http://localhost:3333")
        {
            _apiBaseUrl = apiBaseUrl.TrimEnd('/');
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> TranslateAsync(string text, string sourceLang, string targetLang, string apiKey, string serverToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    throw new ArgumentException("Text cannot be empty");
                }

                // apiKey is optional for server mode, but required for direct ChatGPT
                // (leave validation to server)

                var requestData = new
                {
                    text = text,
                    sourceLang = sourceLang,
                    targetLang = targetLang,
                    apiKey = apiKey,
                    token = serverToken
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/translate", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent);
                    throw new Exception($"Translation failed: {errorResponse?.Error ?? response.StatusCode.ToString()}");
                }

                var translationResponse = JsonSerializer.Deserialize<TranslationResponse>(responseContent);
                
                if (translationResponse?.Success != true)
                {
                    throw new Exception($"Translation failed: {translationResponse?.Error ?? "Unknown error"}");
                }

                return translationResponse.Translation;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Network error: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Request timed out. Please check your internet connection and try again.");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Invalid response from server: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Translation error: {ex.Message}");
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/health");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        // Response models
        private class TranslationResponse
        {
            public bool Success { get; set; }
            public string Translation { get; set; }
            public string SourceLang { get; set; }
            public string TargetLang { get; set; }
            public string OriginalText { get; set; }
            public string Error { get; set; }
        }

        private class ErrorResponse
        {
            public string Error { get; set; }
            public bool Success { get; set; }
        }
    }
} 