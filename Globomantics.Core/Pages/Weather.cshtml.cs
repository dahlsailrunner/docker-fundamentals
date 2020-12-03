using System.Collections.Generic;
using System.Threading.Tasks;
using Globomantics.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Globomantics.Core.Pages
{
    public class WeatherModel : PageModel
    {
        private readonly IApiClient _apiClient;
        public List<WeatherForecastModel> Forecast { get; set; }

        public WeatherModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGet()
        {
            Forecast = await _apiClient.GetWeatherForecastAsync();
        }
    }
}
