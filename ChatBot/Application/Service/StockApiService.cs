using Application.Api.Services;
using Application.Api.Settings;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace Application.Services
{
    public class StockApiService : ApiService, IStockApiService
    {
        public StockApiService(ILogger<ApiService> logger) : base(logger)
        {
        }

        public override RestClient CreateRestClient(string url, RestClient? restClient)
        {
            return restClient ?? new RestClient(url);
        }

        public async Task<byte[]?> Request(string stock, RestClient? restClient = null)
        {
            var apiSettings = new ApiSettings
            {
                RestClient = CreateRestClient("https://stooq.com/", restClient),
                Method = Method.Get,
                Path = "q/l/",
                Headers = new HeadersSettings(new List<DataSettingsBase>()),
                Params = new ParamsSettings(new List<DataSettingsBase>()),
                QueryParams = new QueryParamsSettings(new List<DataSettingsBase>
                {
                    new DataSettingsBase("s", stock),
                    new DataSettingsBase("f", "sd2t2ohlcv"),
                    new DataSettingsBase("h", ""),
                    new DataSettingsBase("e", "csv"),
                }),
            };

            var result = await DownloadFile(apiSettings, string.Empty);

            return result;
        }

    }
}
