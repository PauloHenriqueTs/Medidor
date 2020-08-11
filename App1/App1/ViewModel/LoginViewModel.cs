using App1.Services;
using App1.View;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModel
{
    public class LoginViewModel : ViewModel
    {
        private string _Email { get; set; }
        private string _Password { get; set; }

        private AccountService Service = new AccountService();
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await Login(), () => true);
        }

        private async Task Login()
        {
            var response = await Service.Login(this);

            if (response.IsSuccessStatusCode)
            {
                var Token = await response.Content.ReadAsStringAsync();
                var service = new EnergyMetersService(Token);
                var meters = await service.GetAll();
                await Navigation.PushAsync(new MainView(new MainViewModel(service, meters)));
            }
        }

        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    RaisePropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    RaisePropertyChanged(nameof(Password));
                }
            }
        }
    }
}