using CurrencyExchangeApi.Services.Interfaces;
using CurrencyExchangeApi.DTOs;
using CurrencyExchangeApi.ExternalModels; // Dış API modellerini kullanmak için
using Microsoft.Extensions.Configuration; // API anahtarını okumak için
using Newtonsoft.Json; // JSON deserileştirme için
using System.Net.Http;
using System.Threading.Tasks;
using System; // ArgumentException için

namespace CurrencyExchangeApi.Services.Implementations
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        // HttpClientFactory tarafından sağlanan HttpClient'ı ve IConfiguration'ı alıyoruz
        public ExchangeRateService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            // Program.cs'de yapılandırdığımız isimlendirilmiş HttpClient'ı alıyoruz
            _httpClient = httpClientFactory.CreateClient("ExchangeRateApi");
            _apiKey = configuration["ExchangeRateApi:ApiKey"]; // appsettings.json'dan API anahtarını alıyoruz

            if (string.IsNullOrEmpty(_apiKey))
            {
                throw new InvalidOperationException("ExchangeRateApi:ApiKey is not configured in appsettings.json.");
            }
        }

        public async Task<ExchangeRateDto> GetLatestExchangeRatesAsync(string baseCurrencyCode)
        {
            // Küçük/büyük harf duyarlılığını kaldırmak için büyük harfe çevirelim
            baseCurrencyCode = baseCurrencyCode.ToUpperInvariant();

            // Dış API'nin tam URL'sini oluşturuyoruz
            // BaseAddress zaten ayarlı, bu yüzden sadece API anahtarını ve endpoint'i ekliyoruz.
            var requestUrl = $"{_apiKey}/latest/{baseCurrencyCode}";

            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

            // HTTP yanıtını kontrol et
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                // Gerçek uygulamada daha detaylı loglama ve hata yönetimi yapılabilir
                Console.WriteLine($"Error calling ExchangeRateApi: {response.StatusCode} - {errorContent}");
                throw new HttpRequestException($"Exchangerate-API'den hata yanıtı: {response.StatusCode}. Detay: {errorContent}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            // JSON yanıtını ExternalModel'e dönüştürüyoruz
            var apiResponse = JsonConvert.DeserializeObject<ExchangeRateApiResponse>(jsonResponse);

            // Eğer API yanıtında bir hata varsa (örneğin geçersiz para birimi kodu)
            if (apiResponse == null || apiResponse.Result != "success")
            {
                // API'den gelen spesifik hata mesajını kontrol et ve fırlat
                if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.Result))
                {
                    // Exchangerate-API'nin result alanı "error" olduğunda error-type veriyor
                    // Örneğin: "error-type": "invalid-key" veya "unknown-code"
                    // apiResponse.ErrorType veya apiResponse.ErrorDetails gibi alanlar varsa onları da kullanabiliriz
                    // Şu anki versiyonda bu alanlar yok, sadece "result" değeri "error" oluyor.
                    throw new ArgumentException($"Döviz kuru API'sinden hata: {apiResponse.Result}");
                }
                throw new Exception("Döviz kuru bilgisi alınırken beklenmeyen bir hata oluştu.");
            }

            // ExternalModel'den kendi DTO'muza dönüştürüyoruz
            return new ExchangeRateDto
            {
                BaseCurrency = apiResponse.BaseCode,
                LastUpdateUtc = apiResponse.TimeLastUpdateUtc,
                Rates = apiResponse.ConversionRates
            };
        }
    }
}