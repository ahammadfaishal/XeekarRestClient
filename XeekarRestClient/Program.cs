using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pathoschild.Http.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XeekarRestClient.Dtos;

namespace XeekarRestClient
{
    class Program
    {
       // private static string apiBaseUrl = "https://www.xeekar.com";
        private static string apiBaseUrl = "http://localhost:61814";

        //private static string apiBaseUrl = "http://vts100.linkxtek.com";

        //For Authentication to Xeekar API
        //private static string GetAuthToken(AuthorizationInputDto input)
        //{
        //    string post_data = $"username={input.UserName}&password={input.Password}&grant_type={input.TenancyName}";

        //    string uri = $"{apiBaseUrl}/token";

        //    var response = RestApiPost(uri, string.Empty, post_data);

        //    return response.access_token;
        //}

        //To Create/Update Device To Database and Get Device ID
        private static int UpdateAndGetDeviceID(string token, string imei, string mcid, int modelid, string gsmVersion, string hardwareVersion, string firmwareVersion)
        {
            string post_data = $"imei={imei}&mcid={mcid}&modelid={modelid}&gsmVersion={gsmVersion}&hardwareVersion={hardwareVersion}&firmwareVersion={firmwareVersion}";

            string uri = $"{apiBaseUrl}/api/deviceqc/AddOrUpdateDevice";

            var response = RestApiPost(uri, token, string.Empty);

            return int.Parse(response);
        }



        static void Main(string[] args)
        {
           var token = GetToken(new AuthorizationInputDto() { Password = "body123", UserNameOrEmailAddress = "admin" });
            //var token = GetAuthToken(new AuthorizationInputDto() { Password = "123qwe", UserName = "deviceqc" });

            Console.WriteLine($"access token: {token}");
            GetRestApi(token);
            //RestApiPost(token);
            //int deviceId = UpdateAndGetDeviceID(string.Empty, "IMEI", "MCID", 1, "GSM", "1", "1");

            //Console.WriteLine($"device id: {deviceId}");
            Console.ReadLine();
        }




        private static dynamic RestApiPost(string url, string token, string data)
        {
            // create a request
            HttpWebRequest request = (HttpWebRequest)
            WebRequest.Create(url); request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";

            // turn our request string into a byte stream
            byte[] postBytes = Encoding.ASCII.GetBytes(data);

            // this is important - make sure you specify type this way
            if (!string.IsNullOrEmpty(token))
                request.Headers.Add("Authorization", $"Bearer {token}");

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            Stream requestStream = request.GetRequestStream();

            // now send it
            requestStream.Write(postBytes, 0, postBytes.Length);
            requestStream.Close();

            // grab te response and print it out to the console along with the status code
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

           
            return JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());


        }

        public static void RestApiPost(string token)
        {
            HttpClient httpClient = new HttpClient();

            var uri = "api/user/CreateUser";
            //specify to use TLS 1.2 as default connection
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            httpClient.BaseAddress = new Uri(apiBaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
           // httpClient.DefaultRequestHeaders.Accept.a
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var data = new CreateExternalUserInput()
            {
                Name = "",
                Email = "skdj@slkdfj.com",
                PhoneNumber = "skdjfl",
                Address = "skdjfl",
                IsActive = true
            };

            var values = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(values, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PostAsync(uri, httpContent).Result;

            var contents = response.Content.ReadAsStringAsync().Result;

            dynamic obj = JObject.Parse(contents);
        }

        public static string GetToken(AuthorizationInputDto input)
        {
            HttpClient httpClient = new HttpClient();

            //specify to use TLS 1.2 as default connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            httpClient.BaseAddress = new Uri(apiBaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var values = JsonConvert.SerializeObject(input);
            var httpContent = new StringContent(values, Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("/api/Account/Authenticate", httpContent).Result;

            var contents =  response.Content.ReadAsStringAsync().Result;

            dynamic obj = JObject.Parse(contents);

            return obj.result;
        }

        public static void GetRestApi(string token)
        {
            HttpClient httpClient = new HttpClient();

           // AssignCard(int userId, string cardNumber, DateTime ? endDate)
           // var uri = $"api/card/AssignCard?userId={}&cardNumber={}&endDate=";
            //specify to use TLS 1.2 as default connection
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            //
            httpClient.BaseAddress = new Uri(apiBaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            // httpClient.DefaultRequestHeaders.Accept.a
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");



            var response = httpClient.GetAsync($"api/Card/AssignCard?userId=917&cardNumber=106,44444&endDate=2018-07-29T21:58:39").Result;
            var hskdjfh = response.Content.ReadAsStringAsync().Result;
        }

       
    }
}
