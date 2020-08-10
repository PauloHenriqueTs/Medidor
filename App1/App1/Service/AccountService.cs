using App1.Service;
using App1.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Services
{
    public class AccountService
    {
        private HttpClient client;

        public AccountService()
        {
            ;

            string baseUrl = Settings.getUrl();
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<HttpResponseMessage> Login(LoginViewModel viewModel)
        {
            var model = new LoginDto { Email = viewModel.Email, Password = viewModel.Password };
            var response = await client.PostAsJsonAsync("/api/Account/login", model);

            return response;
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}