using RestSharp;

namespace Application.Interfaces
{
    public interface IStockApiService
    {
        Task<byte[]?> Request(string stock, RestClient? restClient = null);
    }
}
