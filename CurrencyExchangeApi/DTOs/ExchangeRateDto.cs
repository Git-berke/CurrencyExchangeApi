using System.Collections.Generic;

namespace CurrencyExchangeApi.DTOs
{
    // Bu DTO, kendi API'mizden istemciye dönecek sadeleştirilmiş veri yapısıdır.
    public class ExchangeRateDto
    {
        public string BaseCurrency { get; set; } // Örneğin "USD"
        public string LastUpdateUtc { get; set; } // Son güncelleme zamanı
        public Dictionary<string, decimal> Rates { get; set; } // Para birimi kodu -> kur
    }
}