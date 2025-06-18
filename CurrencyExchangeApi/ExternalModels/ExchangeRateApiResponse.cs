using System.Collections.Generic;
using Newtonsoft.Json; // JSON özelliklerini kullanmak için

namespace CurrencyExchangeApi.ExternalModels
{
    // Bu sınıf, Exchangerate-API'den gelen ana JSON yanıtını temsil eder.
    public class ExchangeRateApiResponse
    {
        [JsonProperty("result")] // JSON'daki "result" anahtarını eşler
        public string Result { get; set; }

        [JsonProperty("documentation")]
        public string Documentation { get; set; }

        [JsonProperty("terms_of_use")]
        public string TermsOfUse { get; set; }

        [JsonProperty("time_last_update_unix")]
        public long TimeLastUpdateUnix { get; set; }

        [JsonProperty("time_last_update_utc")]
        public string TimeLastUpdateUtc { get; set; }

        [JsonProperty("time_next_update_unix")]
        public long TimeNextUpdateUnix { get; set; }

        [JsonProperty("time_next_update_utc")]
        public string TimeNextUpdateUtc { get; set; }

        [JsonProperty("base_code")]
        public string BaseCode { get; set; } // Örneğin "USD"

        [JsonProperty("conversion_rates")] // Bu, dinamik anahtarlı bir sözlük olacak
        public Dictionary<string, decimal> ConversionRates { get; set; } // Para birimi kodu -> kur
    }
}