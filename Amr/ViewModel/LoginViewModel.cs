using Amr.Utils;
using Amr.View;
using Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Amr.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        private string _email;

        private Frame nav;
        private static readonly HttpClient client = new HttpClient();
        public ICommand LoginCommand { get; set; }
        private NavigationService navigation { get; set; }

        public LoginViewModel(Frame nav)
        {
            this.nav = nav;
            LoginCommand = new DelegateCommand(async (param) => await login(param));
        }

        public string email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(email));
            }
        }

        private async Task login(object obj)
        {
            PasswordBox pwBox = obj as PasswordBox;
            var secret = pwBox.Password;

            var dto = new LoginDto { Email = email, Password = secret, ConfirmPassword = secret };
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("https://localhost:5001/api/Account/login", dto);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var res = await client.GetAsync("https://localhost:5001/api/EnergyMeters");
                    var resAsString = await res.Content.ReadAsStringAsync();
                    var test = JsonConvert.DeserializeObject<IEnumerable<EnergyMeter>>(resAsString);
                    nav.Content = new MainView(token, test);
                }
            }
            catch (Exception) { }
        }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}