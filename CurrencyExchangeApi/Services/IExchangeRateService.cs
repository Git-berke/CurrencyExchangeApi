using CurrencyExchangeApi.DTOs;
using System.Threading.Tasks;
using System.Collections.Generic; // Dictionary kullanmak için

namespace CurrencyExchangeApi.Services.Interfaces
{
    public interface IExchangeRateService
    {
        // Belirtilen temel para birimine göre tüm döviz kurlarını getirir
        Task<ExchangeRateDto> GetLatestExchangeRatesAsync(string baseCurrencyCode);

        // İsteğe bağlı olarak, belirli bir temelden hedef para birimine dönüşüm oranı
        // Task<decimal?> GetConversionRateAsync(string baseCurrencyCode, string targetCurrencyCode);
    }
}