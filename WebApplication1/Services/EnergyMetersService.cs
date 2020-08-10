using Entities;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApplication1.ViewModel;

namespace WebApplication1.Services
{
    public class EnergyMetersService
    {
        private HttpClient client;

        public EnergyMetersService(string Jwt)
        {
            string baseUrl = "http://localhost:5001";
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Jwt));
        }

        public async Task<List<EnergyMeter>> GetAll(string jwt)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.GetAsync("http://localhost:5001/api/EnergyMeters");
            var ResponseAsString = await Response.Content.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<IEnumerable<EnergyMeter>>(ResponseAsString);
            return test.ToList();
        }

        public async Task<EnergyMeter> GetById(string jwt, string Id)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.GetAsync(String.Format("http://localhost:5001/api/EnergyMeters/{0}", Id));
            var ResponseAsString = await Response.Content.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<EnergyMeter>(ResponseAsString);
            return test;
        }

        public async Task<HttpResponseMessage> Create(string jwt, EnergyMeterCreateViewModel model)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.PostAsJsonAsync("http://localhost:5001/api/EnergyMeters", model);
            return Response;
        }

        public async Task<HttpResponseMessage> GetCount(string jwt, string serialId)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.PostAsJsonAsync("http://localhost:5001/api/EnergyMeters/getCount", serialId);
            return Response;
        }

        public async Task<HttpResponseMessage> Update(string jwt, EnergyMeterUpdateViewModel model)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.PutAsJsonAsync("http://localhost:5001/api/EnergyMeters", model);
            return Response;
        }

        public async Task<HttpResponseMessage> Switch(string jwt, string serialId)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.PostAsJsonAsync("http://localhost:5001/api/EnergyMeters/switch", serialId);
            return Response;
        }

        public async Task<HttpResponseMessage> Delete(string jwt, string serialId)
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", jwt.Replace("\"", string.Empty).Trim()));
            var Response = await client.DeleteAsync(String.Format("http://localhost:5001/api/EnergyMeters/{0}", serialId));
            return Response;
        }
    }
}