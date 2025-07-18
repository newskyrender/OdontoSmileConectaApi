using Integration.Domain.Http.Response;
using Integration.Domain.Http.Request;
using Integration.Domain.Common;
using System.Text;
using System.Text.Json;
// using Seven.Core.Lib.Extensions; - Temporarily disabled
// using Seven.Core.Lib.Gateway; - Temporarily disabled

namespace Integration.Gateway.Api.Service
{
    public class FakeService: ServiceBase
    {
        public readonly HttpClient _httpClient;

        public FakeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<FakeResponse> Get(Guid id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var response = await _httpClient
                .GetAsync($"api-integration/get-fake?id={id}");

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FakeResponse>(content);
        }

        public async Task<IEnumerable<FakeResponse>> GetAll(string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var response = await _httpClient
                .GetAsync($"api-integration/get-list-fake");

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<FakeResponse>>(content);
        }

        public async Task<FakeResponse> Add(FakeRegisterRequest request, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient
                .PostAsync($"api-integration/add-fake", content);

            if (!response.IsSuccessStatusCode) return default;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FakeResponse>(responseContent);
        }

        public async Task<FakeResponse> Update(FakeUpdateRequest request, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient
                .PutAsync($"api-integration/update-fake", content);

            if (!response.IsSuccessStatusCode) return default;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FakeResponse>(responseContent);
        }

        public async Task<FakeResponse> Delete(Guid id, string token)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", token);
            }

            var response = await _httpClient
                .DeleteAsync($"api-integration/delete-fake?id={id}");

            if (!response.IsSuccessStatusCode) return default;

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<FakeResponse>(responseContent);
        }

    }
}

