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

        public MainViewModel(EnergyMetersService Service, List<EnergyMeter> energyMeters)
        {
            service = Service;
            foreach (var item in energyMeters)
            {
                var meterViewModel = new MeterViewModel(item, service, this);
                meterViewModel.Navigation = this.Navigation;
                Meters.Add(meterViewModel);
            }
            CreateCommand = new Command(async () => await Create(), () => true);
        }

        public async Task NavigateToUpdate(MeterViewModel viewModel)
        {
            await Navigation.PushAsync(new UpdateView(new UpdateViewModel(viewModel, service)));
        }

        private async Task Create()
        {
            await Navigation.PushAsync(new CreateView(new CreateViewModel(this.Jwt, service)));
        }

        public ICommand CreateCommand { get; set; }
        private string Jwt { get; set; }
        public ObservableCollection<MeterViewModel> Meters { get; set; } = new ObservableCollection<MeterViewModel>();
    }
}