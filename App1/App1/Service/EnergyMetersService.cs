using App1.Dto;
using App1.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace App1.Services
{
    public class EnergyMetersService
    {
        private HttpClient client;

        public EnergyMetersService(string Jwt)
        {
            string baseUrl = Settings.getUrl();
            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);

            client.DefaultRequestHeaders.Add("Authorization", String.Format("Bearer {0}", Jwt.Replace("\"", string.Empty).Trim()));
        }

        public async Task<HttpResponseMessage> Create(EnergyMeterCreateDto model)
        {
            var Response = await client.PostAsJsonAsync("/api/EnergyMeters", model);
            return Response;
        }

        public async Task<List<EnergyMeter>> GetAll()
        {
            var Response = await client.GetAsync("/api/EnergyMeters");
            var ResponseAsString = await Response.Content.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<IEnumerable<EnergyMeter>>(ResponseAsString);

            return test.ToList();
        }

        public async Task<HttpResponseMessage> Update(EnergyMeterCreateDto model)
        {
            var Response = await client.PutAsJsonAsync("/api/EnergyMeters", model);
            return Response;
        }

        public async Task<EnergyMeter> GetById(string Id)
        {
            var Response = await client.GetAsync(String.Format("/api/EnergyMeters/{0}", Id));
            var ResponseAsString = await Response.Content.ReadAsStringAsync();
            var test = JsonConvert.DeserializeObject<EnergyMeter>(ResponseAsString);
            return test;
        }

        public async Task<HttpResponseMessage> GetCount(string serialId)
        {
            var Response = await client.PostAsJsonAsync("/api/EnergyMeters/getCount", serialId);
            return Response;
        }

        public async Task<HttpResponseMessage> Switch(string serialId)
        {
            var Response = await client.PostAsJsonAsync("/api/EnergyMeters/switch", serialId);
            return Response;
        }

        public async Task<HttpResponseMessage> Delete(string serialId)
        {
            var Response = await client.DeleteAsync(String.Format("/api/EnergyMeters/{0}", serialId));
            return Response;
        }
    }
}