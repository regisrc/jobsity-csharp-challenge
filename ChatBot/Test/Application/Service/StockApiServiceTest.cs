using Application.Api.Services;
using Application.Api.Settings;
using Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text;

namespace Test.Application.Service
{
    public class StockApiServiceTest
    {
        private readonly Mock<ILogger<ApiService>> _logger;
        private readonly StockApiService _stockApiService;

        public StockApiServiceTest()
        {
            _logger = new Mock<ILogger<ApiService>>();

            _stockApiService = new StockApiService(_logger.Object);
        }

        [Fact]
        public void SetAuthenticationHeader_ShouldNot_AddAuthHeader()
        {
            var request = new RestRequest(string.Empty);

            _stockApiService.SetAuthenticationHeader(request, null);

            var parameters = request.Parameters;

            Assert.Empty(parameters);
        }

        [Fact]
        public void SetAuthenticationHeader_Should_AddAuthHeader()
        {
            var request = new RestRequest(string.Empty);
            var token = "Bearer Token";
            var authenticationHeaderName = "Authorization";

            _stockApiService.SetAuthenticationHeader(request, token);

            var parameters = request.Parameters;

            Assert.Single(parameters);

            var param = parameters.TryFind(authenticationHeaderName);

            Assert.NotNull(param);
            Assert.Equal(token, param.Value);
        }

        [Fact]
        public void SetHeaders_Should_AddHeaders()
        {
            var request = new RestRequest(string.Empty);
            var headers = new List<DataSettingsBase>
            {
                new DataSettingsBase("Test1", "Test1"),
                new DataSettingsBase("Test2", "Test2"),
                new DataSettingsBase("Test3", "Test3")
            };

            _stockApiService.SetHeaders(request, headers);

            var parameters = request.Parameters;

            Assert.Equal(headers.Count, parameters.Count);

            foreach (var header in headers)
            {
                var param = parameters.TryFind(header.Key);

                Assert.NotNull(param);
                Assert.Equal(header.Value, param.Value);
            }
        }

        [Fact]
        public void SetHeaders_ShouldNot_AddHeaders()
        {
            var request = new RestRequest(string.Empty);
            var headers = new List<DataSettingsBase>();

            _stockApiService.SetHeaders(request, headers);

            var parameters = request.Parameters;

            Assert.Empty(parameters);
        }

        [Fact]
        public void SetParams_Should_AddParams()
        {
            var request = new RestRequest(string.Empty);
            var @params = new List<DataSettingsBase>
            {
                new DataSettingsBase("Test1", "Test1"),
                new DataSettingsBase("Test2", "Test2"),
                new DataSettingsBase("Test3", "Test3")
            };

            _stockApiService.SetParams(request, @params);

            var parameters = request.Parameters;

            Assert.Equal(@params.Count, parameters.Count);

            foreach (var item in @params)
            {
                var param = parameters.TryFind(item.Key);

                Assert.NotNull(param);
                Assert.Equal(item.Value, param.Value);
            }
        }

        [Fact]
        public void SetQueryParams_Should_AddParams()
        {
            var request = new RestRequest(string.Empty);
            var queryParams = new List<DataSettingsBase>
            {
                new DataSettingsBase("Test1", "Test1"),
                new DataSettingsBase("Test2", "Test2"),
                new DataSettingsBase("Test3", "Test3")
            };

            _stockApiService.SetQueryParams(request, queryParams);

            var parameters = request.Parameters;

            Assert.Equal(queryParams.Count, parameters.Count);

            foreach (var item in queryParams)
            {
                var param = parameters.TryFind(item.Key);

                Assert.NotNull(param);
                Assert.Equal(item.Value, param.Value);
            }
        }

        [Fact]
        public void SetHeaders_ShouldNot_AddParams()
        {
            var request = new RestRequest(string.Empty);
            var @params = new List<DataSettingsBase>();

            _stockApiService.SetHeaders(request, @params);

            var parameters = request.Parameters;

            Assert.Empty(parameters);
        }

        [Fact]
        public void SetQueryParams_ShouldNot_AddParams()
        {
            var request = new RestRequest(string.Empty);
            var @params = new List<DataSettingsBase>();

            _stockApiService.SetQueryParams(request, @params);

            var parameters = request.Parameters;

            Assert.Empty(parameters);
        }

        [Fact]
        public void PrepareMessageToJsonBody_Should_Return_Json()
        {
            var person = new { Name = "Regis", Occupation = "Programmer and Teacher" };

            var json = _stockApiService.PrepareMessageToJsonBody(person);

            Assert.Equal("{\"Name\":\"Regis\",\"Occupation\":\"Programmer and Teacher\"}", json);
        }

        [Fact]
        public void SetBody_ShouldSet_Body()
        {
            var request = new RestRequest(string.Empty);
            var person = new { Name = "Regis", Occupation = "Programmer and Teacher" };

            _stockApiService.SetBody(request, person);

            var parameters = request.Parameters;

            Assert.Single(parameters);

            var body = parameters.TryFind("");

            Assert.NotNull(body);
            Assert.Equal("{\"Name\":\"Regis\",\"Occupation\":\"Programmer and Teacher\"}", body.Value);
        }

        [Fact]
        public void GetPolicyResponse_ShouldDo_Request()
        {
            var url = "http://localhost/api/user/*";
            var request = new RestRequest(string.Empty);

            var mockHttp = new MockHttpMessageHandler();

            var json = "{'name' : 'Test McGee'}";

            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", json);

            var client = new RestClient(new RestClientOptions { BaseUrl = new Uri("http://localhost/api/user/"), ConfigureMessageHandler = _ => mockHttp });

            var apiSettings = new ApiSettings
            {
                RestClient = client,
                Method = Method.Get,
                Path = "1234",
                Headers = new HeadersSettings(new List<DataSettingsBase>
                {
                    new DataSettingsBase("Teste", "Teste")
                }),
                Params = new ParamsSettings(new List<DataSettingsBase>{
                    new DataSettingsBase("Teste", "Teste")
                })
            };

            var pollySettings = new PollySettings
            {
                RetryCondition = (x => x.StatusCode == HttpStatusCode.BadRequest),
                RetryCount = 5,
                DelaySeconds = 10
            };

            var response = _stockApiService.GetPolicyResponse(apiSettings, pollySettings, request).GetAwaiter().GetResult().Result;

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(json, response.Content);
        }

        [Fact]
        public void PrepareRequest_ShouldDo_Request()
        {
            var url = "http://localhost/api/user/*";
            var request = new RestRequest(string.Empty);

            var mockHttp = new MockHttpMessageHandler();

            var json = "{'name' : 'Test McGee'}";

            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", json);

            var client = new RestClient(new RestClientOptions { BaseUrl = new Uri("http://localhost/api/user/"), ConfigureMessageHandler = _ => mockHttp });

            var apiSettings = new ApiSettings
            {
                RestClient = client,
                Method = Method.Get,
                Path = "1234",
                Headers = new HeadersSettings(new List<DataSettingsBase>
                {
                    new DataSettingsBase("Teste", "Teste")
                }),
                Params = new ParamsSettings(new List<DataSettingsBase>{
                    new DataSettingsBase("Teste", "Teste")
                }),
                QueryParams = new QueryParamsSettings(new List<DataSettingsBase>{
                    new DataSettingsBase("Teste", "Teste")
                })
            };

            var pollySettings = new PollySettings
            {
                RetryCondition = (x => x.StatusCode == HttpStatusCode.BadRequest),
                RetryCount = 5,
                DelaySeconds = 10
            };

            var response = _stockApiService.PrepareRequest(apiSettings, pollySettings, request).GetAwaiter().GetResult();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(json, response.Content);
        }

        [Fact]
        public void DownloadFile_ShouldDo_Request()
        {
            var url = "http://localhost/api/user/*";
            var request = new RestRequest(string.Empty);

            var mockHttp = new MockHttpMessageHandler();

            var json = "{'name' : 'Test McGee'}";

            mockHttp.When(url)
                    .Respond(HttpStatusCode.OK, "application/json", json);

            var client = new RestClient(new RestClientOptions { BaseUrl = new Uri("http://localhost/api/user/"), ConfigureMessageHandler = _ => mockHttp });

            var apiSettings = new ApiSettings
            {
                RestClient = client,
                Method = Method.Get,
                Path = "1234",
                Headers = new HeadersSettings(new List<DataSettingsBase>
                {
                    new DataSettingsBase("Teste", "Teste")
                }),
                Params = new ParamsSettings(new List<DataSettingsBase>{
                    new DataSettingsBase("Teste", "Teste")
                }),
                QueryParams = new QueryParamsSettings(new List<DataSettingsBase>{
                    new DataSettingsBase("Teste", "Teste")
                })
            };

            var pollySettings = new PollySettings
            {
                RetryCondition = (x => x.StatusCode == HttpStatusCode.BadRequest),
                RetryCount = 5,
                DelaySeconds = 10
            };

            var response = _stockApiService.DownloadFile(apiSettings, string.Empty).GetAwaiter().GetResult();

            var result = Encoding.Default.GetString(response);

            Assert.Equal(json, result);
        }
    }
}
