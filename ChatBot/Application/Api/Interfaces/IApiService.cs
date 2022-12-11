using Application.Api.Settings;
using RestSharp;

namespace Application.Api.Interfaces
{
    public interface IApiService
    {
        Task<byte[]?> DownloadFile(ApiSettings apiSettings, object body);

        Task<RestResponse<string>> PrepareRequest(ApiSettings apiSettings, PollySettings pollySettings, object body);
    }
}
