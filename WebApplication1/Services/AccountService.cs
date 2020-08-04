using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication1.ViewModel;

namespace WebApplication1.Services
{
    public class AccountService
    {
        private HttpClient client;

        public AccountService()
        {
            string baseUrl = "https://localhost:5001";
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<HttpResponseMessage> Login(LoginViewModel model)
        {
            var response = await client.PostAsJsonAsync("/api/Account/login", model);
            return response;
        }

        public async Task<HttpResponseMessage> Register(RegisterViewModel model)
        {
            var response = await client.PostAsJsonAsync("/api/Account/register", model);
            return response;
        }
    }
}