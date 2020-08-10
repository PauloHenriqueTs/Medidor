using App1.Dto;
using App1.Services;
using App1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace App1.ViewModel
{
    public class MainViewModel : ViewModel
    {
        private EnergyMetersService service;

        public MainViewModel(string jwt, List<EnergyMeter> energyMeters)
        {
            Jwt = jwt;
            service = new EnergyMetersService(Jwt);
            foreach (var item in energyMeters)
            {
                Meters.Add(new MeterViewModel(item, service, this));
            }
            CreateCommand = new Command(async () => await Create(), () => true);
        }

        private async Task Create()
        {
            await Navigation.PushAsync(new CreateView(new CreateViewModel(this.Jwt, service)));
        }

        public ICommand CreateCommand { get; set; }
        private string Jwt { get; set; }
        public ObservableCollection<MeterViewModel> Meters { get; set; } = new ObservableCollection<MeterViewModel>();

        public string getJwt()
        {
            return this.Jwt;
        }
    }
}