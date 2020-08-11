using App1.Dto;
using App1.Dto.ValueObject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using App1.Services;
using System.Threading.Tasks;
using App1.View;

namespace App1.ViewModel
{
    public class MeterViewModel : ViewModel
    {
        private EnergyMetersService service;
        private MainViewModel mainView;

        public MeterViewModel(EnergyMeter item, EnergyMetersService Service, MainViewModel MainView)
        {
            mainView = MainView;
            service = Service;
            Type = item.Type;
            SerialId = item.SerialId;
            Count = item.Count;
            UserId = item.UserId;
            SwitchState = item.SwitchState;
            foreach (var i in item.Meters)
            {
                MetersOfPole.Add(new MeterOfPoleViewModel { MeterId = i.MeterId });
            }
            DeleteCommand = new Command(async () => await Delete(), () => true);
            SwitchCommand = new Command(async () => await Switch(), () => true);
            GetCountCommand = new Command(async () => await GetCount(), () => true);
            UpdateCommand = new Command(async () => await Update(), () => true);
        }

        private async Task Update()
        {
            await mainView.NavigateToUpdate(this);
            // await Navigation.PushAsync(new UpdateView(new UpdateViewModel(this, service)));
        }

        private async Task GetCount()
        {
            var response = await service.GetCount(this.SerialId);
            if (response.IsSuccessStatusCode)
            {
                var meter = await service.GetById(this.SerialId);
                this.Count = meter.Count;
            }
        }

        private async Task Switch()
        {
            await service.Switch(this.SerialId);
        }

        private async Task Delete()
        {
            var response = await service.Delete(this.SerialId);
            if (response.IsSuccessStatusCode)
            {
                mainView.Meters.Remove(this);
            }
        }

        public ICommand DeleteCommand { get; set; }
        public ICommand SwitchCommand { get; set; }
        public ICommand GetCountCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        private string _SerialId { get; set; }

        public string SerialId
        {
            get { return _SerialId; }
            set
            {
                if (_SerialId != value)
                {
                    _SerialId = value;
                    RaisePropertyChanged(nameof(SerialId));
                }
            }
        }

        private string _UserId { get; set; }

        public string UserId
        {
            get { return _UserId; }
            set
            {
                if (_UserId != value)
                {
                    _UserId = value;
                    RaisePropertyChanged(nameof(UserId));
                }
            }
        }

        public TypeOfEnergyMeter Type { get; set; }

        public ObservableCollection<MeterOfPoleViewModel> MetersOfPole { get; set; } = new ObservableCollection<MeterOfPoleViewModel>();

        private string _Count { get; set; }

        public string Count
        {
            get { return _Count; }
            set
            {
                if (_Count != value)
                {
                    _Count = value;
                    RaisePropertyChanged(nameof(Count));
                }
            }
        }

        private bool _SwitchState { get; set; }

        public bool SwitchState
        {
            get { return _SwitchState; }
            set
            {
                if (_SwitchState != value)
                {
                    _SwitchState = value;
                    RaisePropertyChanged(nameof(SwitchState));
                }
            }
        }
    }
}