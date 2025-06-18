using CurrencyExchangeApi.DTOs;
using CurrencyExchangeApi.Services.Interfaces; // Servis arayüzünü kullanmak için
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CurrencyExchangeApi.Controllers
{
    [Route("api/[controller]")] // API URL yolu: /api/ExchangeRates
    [ApiController] // Bu sınıfın bir API kontrolcüsü olduğunu belirtir
    public class ExchangeRatesController : ControllerBase
    {
        private readonly IExchangeRateService _exchangeRateService;

        // IExchangeRateService'i bağımlılık enjeksiyonu ile alıyoruz
        public ExchangeRatesController(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        // GET: api/ExchangeRates/latest/{baseCurrencyCode}
        // Örneğin: api/ExchangeRates/latest/USD
        [HttpGet("latest/{baseCurrencyCode}")]
        public async Task<ActionResult<ExchangeRateDto>> GetLatestRates(string baseCurrencyCode)
        {
            if (string.IsNullOrEmpty(baseCurrencyCode))
            {
                return BadRequest("Temel para birimi kodu boş olamaz.");
            }

            try
            {
                var rates = await _exchangeRateService.GetLatestExchangeRatesAsync(baseCurrencyCode);
                if (rates == null)
                {
                    // Bu durum genellikle dış API'nin geçersiz bir kod için null veya boş yanıt vermesiyle oluşur
                    return NotFound($"'{baseCurrencyCode}' için döviz kuru bilgisi bulunamadı veya geçersiz bir kod.");
                }
                return Ok(rates); // 200 OK yanıtı ile döviz kurlarını döndür
            }
            catch (ArgumentException ex) // Servisten fırlatılan iş mantığı hatalarını yakala
            {
                return BadRequest(ex.Message); // Örneğin, "Döviz kuru API'sinden hata: invalid-code"
            }
            catch (HttpRequestException ex) // Dış API ile iletişim hatalarını yakala
            {
                return StatusCode(503, $"Dış döviz kuru servisiyle iletişimde hata: {ex.Message}"); // 503 Service Unavailable
            }
            catch (Exception) // Beklenmeyen diğer hataları yakala
            {
                return StatusCode(500, "Döviz kuru bilgisi alınırken beklenmeyen bir hata oluştu."); // 500 Internal Server Error
            }
        }

        // İsteğe bağlı: Belirli bir temelden hedefe dönüşüm için
        // GET: api/ExchangeRates/convert/USD/TRY
        // [HttpGet("convert/{baseCurrencyCode}/{targetCurrencyCode}")]
        // public async Task<ActionResult<decimal?>> GetConversionRate(string baseCurrencyCode, string targetCurrencyCode)
        // {
        //     try
        //     {
        //         var rate = await _exchangeRateService.GetConversionRateAsync(baseCurrencyCode, targetCurrencyCode);
        //         if (rate == null)
        //         {
        //             return NotFound("Dönüşüm kuru bulunamadı.");
        //         }
        //         return Ok(rate);
        //     }
        //     catch (Exception)
        //     {
        //         return StatusCode(500, "Dönüşüm kuru alınırken hata oluştu.");
        //     }
        // }
    }
}