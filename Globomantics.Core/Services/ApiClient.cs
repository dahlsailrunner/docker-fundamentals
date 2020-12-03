using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Globomantics.Core.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _cli;

        public ApiClient(HttpClient cli, IConfiguration config)
        {
            _cli = cli;
            _cli.BaseAddress = new Uri(config.GetValue<string>("ApiBaseUrl"));
        }

        public async Task<List<WeatherForecastModel>> GetWeatherForecastAsync()
        {
            return await _cli.GetFromJsonAsync<List<WeatherForecastModel>>("v1/weatherforecast");
        }
    }
}
