using Entities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class EnergyMetersService
    {
        private HttpClient client;

        public EnergyMetersService(string Jwt)
        {
            string baseUrl = "https://localhost:5001";
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Jwt));
        }

        public async Task<List<EnergyMeter>> GetAll(string jwt)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.GetAsync("https://localhost:5001/api/EnergyMeters");
            var ResponseAsString = await Response.Content.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<IEnumerable<EnergyMeter>>(ResponseAsString);
            return test.ToList();
        }
    }
}