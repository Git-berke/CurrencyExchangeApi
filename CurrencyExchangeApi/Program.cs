using CurrencyExchangeApi.Services.Implementations;
using CurrencyExchangeApi.Services.Interfaces;
using Microsoft.Extensions.Configuration; // IConfiguration'ý kullanabilmek için
using System; // TimeSpan için

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// HttpClientFactory'yi yapýlandýrma
builder.Services.AddHttpClient("ExchangeRateApi", client =>
{
    // appsettings.json'dan okuduðumuz BaseUrl'i kullanýyoruz
    client.BaseAddress = new Uri(builder.Configuration["ExchangeRateApi:BaseUrl"]);
    // Bazý API'ler User-Agent veya diðer varsayýlan baþlýklarý isteyebilir.
    // client.DefaultRequestHeaders.Add("User-Agent", "YourApp-CurrencyExchange");
    client.Timeout = TimeSpan.FromSeconds(10); // API yanýt vermesi için maksimum bekleme süresi
});

builder.Services.AddHttpClient("ExchangeRateApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExchangeRateApi:BaseUrl"]);
    client.Timeout = TimeSpan.FromSeconds(10);
});

// Döviz Kuru Servisini baðýmlýlýk enjeksiyonuna ekliyoruz
// AddScoped: Her HTTP isteði için ExchangeRateService'in yeni bir örneði oluþturulur.
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
