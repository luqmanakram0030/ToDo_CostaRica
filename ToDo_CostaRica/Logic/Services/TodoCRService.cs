using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ToDo_CostaRica.Infrastructure;
using ToDoCR.SharedDomain;


namespace ToDo_CostaRica.Services
{
    //var response = await _restClient.GetAsync<PostObject>("https://jsonplaceholder.typicode.com/posts/1");

    public interface IRestClient
    {
        Task<T> GetAsync<T>(string url, bool useAuthToken = false);

        Task<T> PostAsync<T>(string url, object payload, bool useAuthToken = false);

        Task<T> PutAsync<T>(string url, object payload, bool useAuthToken = false);

        Task<T> DeleteAsync<T>(string url, bool useAuthToken = false);
    }

    public class RestClient : IRestClient
    {
        private string _authToken;
        private readonly string urlService;
        private readonly HttpClient httpClient;

        //public RestClient(string urlService = "https://zf7c3vxc-44356.use2.devtunnels.ms/api")
        public RestClient(string urlService = "https://todocr-api-hyaghgg0e5btcbgh.canadacentral-01.azurewebsites.net/api")
        {
            this.urlService = urlService;
            SetAuthToken();
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                var httpClientHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                };
                httpClient = new HttpClient(httpClientHandler, true);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpClient.Timeout = TimeSpan.FromSeconds(10);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while Sending Request : " + ex.Message);
            }
        }

        public void SetAuthToken()
        {
            this._authToken = Locator.Instance.User?.Id.HasValue == true ? Locator.Instance.User.Token.ToString() : "temporal";
        }

        //public static RestClient Instance
        //{
        //    get
        //    {
        //        return _instance;
        //    }
        //}

        private void Dispose()
        {
            httpClient.Dispose();
        }

        private Task<HttpResponseMessage> RequestAsync(HttpMethod method, string url, object payload = null)
        {
            try
            {
                var request = PrepareRequest(method, url, payload);

                //if (_authToken != default(Guid))
                //{
                request.Headers.Add("Authorization", $"{(Locator.Instance.User?.Id.HasValue == true ? Locator.Instance.User.Token.ToString() : "temporal")}|{DateTime.UtcNow.Ticks}".Encriptar());
                //}
                System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
                return httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, tokenSource.Token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while Sending Request : " + ex.Message);
            }

            //else
            //    Application.Current.MainPage = new Views.HomePages.ErrorPage();
            return null;
        }

        private HttpRequestMessage PrepareRequest(HttpMethod method, string url, object payload)
        {
            try
            {
                var uri = PrepareUri(url);
                var request = new HttpRequestMessage(method, uri);
                if (payload != null)
                {
                    var json = JsonConvert.SerializeObject(payload);
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                return request;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error while Sending Request : " + ex.Message);
            }
            return null;
        }

        private Uri PrepareUri(string url)
        {
            return new Uri(this.urlService + url);
        }

        private readonly Action<HttpStatusCode, string> _defaultErrorHandler = (statusCode, body) =>
            {
                if (statusCode < HttpStatusCode.OK || statusCode >= HttpStatusCode.BadRequest)
                {
                    Debug.WriteLine(string.Format("Request responded with status code={0}, response={1}", statusCode, body));
                }
            };

        private void HandleIfErrorResponse(HttpStatusCode statusCode, string content, Action<HttpStatusCode, string> errorHandler = null)
        {
            if (errorHandler != null)
            {
                errorHandler(statusCode, content);
            }
            else
            {
                _defaultErrorHandler(statusCode, content);
            }
        }

        private T GetValue<T>(String value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public string GetResponse { get; set; }

        public async Task<T> GetAsync<T>(string url, bool useAuthToken = true)
        {
            try
            {
                HttpResponseMessage response = await RequestAsync(HttpMethod.Get, url).ConfigureAwait(false);
                GetResponse = response.ToString();
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                if (typeof(T) == typeof(string))
                {
                    return GetValue<T>(content);
                }
                //#region Reset AuthToken
                //if (useAuthToken && _authToken != default(Guid))
                //    _authToken = default(Guid);
                //#endregion
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (System.Net.WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GET Request :" + ex.Message);
            }
            return default(T);
        }

        public async Task<T> PostAsync<T>(string url, object payload = null, bool useAuthToken = true)
        {
            try
            {
                HttpResponseMessage response = await RequestAsync(HttpMethod.Post, url, payload).ConfigureAwait(false);
                if (response != null)
                {
                    string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    HandleIfErrorResponse(response.StatusCode, content);
                    if (typeof(T) == typeof(string))
                    {
                        return GetValue<T>(content);
                    }
                    //#region Reset AuthToken
                    //if (useAuthToken && _authToken != default(Guid))
                    //    _authToken = default(Guid);
                    //#endregion
                    return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Error in POST Request :" + ex.Message);
            }
            return default(T);
        }

        public async Task<T> PutAsync<T>(string url, object payload, bool useAuthToken = true)
        {
            try
            {
                HttpResponseMessage response = await RequestAsync(HttpMethod.Put, url, payload).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                if (typeof(T) == typeof(string))
                {
                    return GetValue<T>(content);
                }
                //#region Reset AuthToken
                //if (useAuthToken && _authToken != default(Guid))

                //    _authToken = default(Guid);
                //#endregion
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Error in PUT Request :" + ex.Message);
            }
            return default(T);
        }

        public async Task<T> DeleteAsync<T>(string url, bool useAuthToken = true)
        {
            try
            {
                HttpResponseMessage response = await RequestAsync(HttpMethod.Delete, url).ConfigureAwait(false);
                string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                HandleIfErrorResponse(response.StatusCode, content);
                if (typeof(T) == typeof(string))
                {
                    return GetValue<T>(content);
                }
                //#region Reset AuthToken
                //if (useAuthToken && _authToken != default(Guid))
                //    _authToken = default(Guid);
                //#endregion
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("Error in DELETE Request :" + ex.Message);
            }
            return default(T);
        }
    }
}