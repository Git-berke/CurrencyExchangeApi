using CurrencyExchangeApi.Services.Implementations;
using CurrencyExchangeApi.Services.Interfaces;
using Microsoft.Extensions.Configuration; // IConfiguration'� kullanabilmek i�in
using System; // TimeSpan i�in

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// HttpClientFactory'yi yap�land�rma
builder.Services.AddHttpClient("ExchangeRateApi", client =>
{
    // appsettings.json'dan okudu�umuz BaseUrl'i kullan�yoruz
    client.BaseAddress = new Uri(builder.Configuration["ExchangeRateApi:BaseUrl"]);
    // Baz� API'ler User-Agent veya di�er varsay�lan ba�l�klar� isteyebilir.
    // client.DefaultRequestHeaders.Add("User-Agent", "YourApp-CurrencyExchange");
    client.Timeout = TimeSpan.FromSeconds(10); // API yan�t vermesi i�in maksimum bekleme s�resi
});

builder.Services.AddHttpClient("ExchangeRateApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ExchangeRateApi:BaseUrl"]);
    client.Timeout = TimeSpan.FromSeconds(10);
});

// D�viz Kuru Servisini ba��ml�l�k enjeksiyonuna ekliyoruz
// AddScoped: Her HTTP iste�i i�in ExchangeRateService'in yeni bir �rne�i olu�turulur.
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
