using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globomantics.Core.Services
{
    public interface IApiClient
    {
        Task<List<WeatherForecastModel>> GetWeatherForecastAsync();
    }
}
