# CurrencyExchangeApi

This project is an ASP.NET Core Web API application that fetches real-time exchange rates from an external API (Exchangerate-API) and serves them through its own RESTful API. Users can access up-to-date currency exchange rates based on a specified base currency.

## Project Goal

* To learn how to integrate an external third-party API (Exchangerate-API) securely and efficiently.
* To manage external API requests using `HttpClientFactory`.
* To transform complex incoming JSON responses into C# models and create simplified DTOs for our own API.
* To reinforce the ASP.NET Core Web API structure (Controller, Service, DTO, ExternalModels).
* To securely manage sensitive information like API keys (using User Secrets).

## Technologies Used

* **ASP.NET Core 8.0 Web API:** The framework for developing the backend API.
* **Exchangerate-API:** The external service providing up-to-date exchange rates.
* **HttpClientFactory:** For robust and managed HTTP requests.
* **Newtonsoft.Json:** For JSON serialization/deserialization operations.
* **Swagger/OpenAPI:** For API documentation and interactive testing interface.
* **User Secrets:** For securely storing sensitive information (API key) in the development environment.

## Setup and Running

Follow these steps to get the project up and running on your local machine.

### Prerequisites

* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/vs/community/) (or a compatible IDE/code editor)
* A **free API Key** obtained from [Exchangerate-API](https://www.exchangerate-api.com/).

### Steps

1.  **Clone the Repository:**
    ```bash
    git clone [https://github.com/YOUR_USERNAME/CurrencyExchangeApi.git](https://github.com/YOUR_USERNAME/CurrencyExchangeApi.git)
    cd CurrencyExchangeApi
    ```
    *Replace `YOUR_USERNAME` with your actual GitHub username.*

2.  **Configure Your API Key Securely (User Secrets):**
    * In Visual Studio, right-click on the `CurrencyExchangeApi.csproj` project (not the solution).
    * Select `Manage User Secrets`.
    * In the `secrets.json` file that opens, paste the following JSON structure and replace `YOUR_EXCHANGERATE_API_KEY` with your actual Exchangerate-API key:

        ```json
        {
          "ExchangeRateApi": {
            "ApiKey": "YOUR_EXCHANGERATE_API_KEY"
          }
        }
        ```
    * Remember to remove or comment out the `ExchangeRateApi:ApiKey` line from your `appsettings.json` file to ensure it's not committed to Git.

3.  **Restore NuGet Packages:**
    * In Visual Studio, right-click on the `CurrencyExchangeApi` project in Solution Explorer and select `Restore NuGet Packages`. Alternatively, from your Terminal/Command Prompt in the project directory, run `dotnet restore`.

4.  **Run the Project:**
    * In Visual Studio, press `F5` or select `Debug > Start Debugging`.
    * Alternatively, from your Terminal/Command Prompt in the project directory, run `dotnet run`.

    Once the application starts, your browser will automatically open to the Swagger UI (typically `https://localhost:[PORT]/swagger/index.html`).

## API Endpoints

Explore and test the API endpoints via Swagger UI once the project is running.

### Exchange Rates

* **`GET /api/ExchangeRates/latest/{baseCurrencyCode}`**
    * **Description:** Returns the latest exchange rates based on the specified base currency (e.g., `USD`, `EUR`, `TRY`).
    * **Parameters:**
        * `baseCurrencyCode` (string, Path): The 3-letter code of the base currency.
    * **Example Request:** `GET https://localhost:[PORT]/api/ExchangeRates/latest/USD`
    * **Example Response:**
        ```json
        {
          "baseCurrency": "USD",
          "lastUpdateUtc": "Thu, 20 Jun 2024 00:00:01 +0000",
          "rates": {
            "USD": 1.0,
            "TRY": 32.50,
            "EUR": 0.93
            // ... other rates
          }
        }
        ```
asd
## Error Handling

The API will return informative HTTP status codes and messages for potential errors:

* **`400 Bad Request`**: For invalid input (e.g., empty base currency code).
* **`404 Not Found`**: If exchange rate information for a specific currency code cannot be found or the code is invalid.
* **`503 Service Unavailable`**: When there is an issue communicating with the external exchange rate service.
* **`500 Internal Server Error`**: For unexpected server-side errors.

## Contributing

If you'd like to contribute, feel free to submit a pull request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for more details (if you create a `LICENSE` file).
