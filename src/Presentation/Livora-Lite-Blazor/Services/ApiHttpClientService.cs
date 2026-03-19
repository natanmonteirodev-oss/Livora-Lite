using System.Net.Http.Json;
using System.Text.Json;

namespace Livora_Lite_Blazor.Services
{
    /// <summary>
    /// Generic HTTP Client Service for consuming Livora-Lite-API
    /// Handles authentication, request/response serialization, and error handling
    /// </summary>
    public class ApiHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiHttpClientService> _logger;
        private string? _authToken;

        public ApiHttpClientService(IHttpClientFactory httpClientFactory, ILogger<ApiHttpClientService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("API");
            _logger = logger;
        }

        /// <summary>
        /// Set the authentication token for subsequent requests
        /// </summary>
        public void SetAuthToken(string token)
        {
            _authToken = token;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Clear the authentication token
        /// </summary>
        public void ClearAuthToken()
        {
            _authToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// GET request
        /// </summary>
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                else
                {
                    _logger.LogError($"GET request failed: {endpoint} - Status: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GET request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// GET request returning raw response
        /// </summary>
        public async Task<HttpResponseMessage> GetRawAsync(string endpoint)
        {
            try
            {
                return await _httpClient.GetAsync(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GET request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// POST request
        /// </summary>
        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"POST request failed: {endpoint} - Status: {response.StatusCode} - Error: {error}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in POST request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// POST request returning raw response
        /// </summary>
        public async Task<HttpResponseMessage> PostRawAsync(string endpoint, object data)
        {
            try
            {
                return await _httpClient.PostAsJsonAsync(endpoint, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in POST request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// PUT request
        /// </summary>
        public async Task<T?> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"PUT request failed: {endpoint} - Status: {response.StatusCode} - Error: {error}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in PUT request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// PUT request returning raw response
        /// </summary>
        public async Task<HttpResponseMessage> PutRawAsync(string endpoint, object data)
        {
            try
            {
                return await _httpClient.PutAsJsonAsync(endpoint, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in PUT request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// DELETE request
        /// </summary>
        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"DELETE request failed: {endpoint} - Status: {response.StatusCode}");
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DELETE request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// DELETE request returning raw response
        /// </summary>
        public async Task<HttpResponseMessage> DeleteRawAsync(string endpoint)
        {
            try
            {
                return await _httpClient.DeleteAsync(endpoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DELETE request to {endpoint}");
                throw;
            }
        }

        /// <summary>
        /// Generic request method with custom HttpMethod
        /// </summary>
        public async Task<T?> SendAsync<T>(string endpoint, HttpMethod method, object? data = null)
        {
            try
            {
                var request = new HttpRequestMessage(method, endpoint);
                
                if (data != null)
                {
                    request.Content = JsonContent.Create(data);
                }

                var response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<T>();
                }
                else
                {
                    _logger.LogError($"{method} request failed: {endpoint} - Status: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in {method} request to {endpoint}");
                throw;
            }
        }
    }
}
