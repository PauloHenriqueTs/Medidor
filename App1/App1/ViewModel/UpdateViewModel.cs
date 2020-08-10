using App1.Dto;
using App1.Dto.ValueObject;
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
    public class UpdateViewModel : ViewModel
    {
        
        private EnergyMetersService Service;

        public UpdateViewModel( MeterViewModel meter, EnergyMetersService service)
        {
            SerialId = meter.SerialId;
            Service = service;
            
            AddMeterOfPoleCommand = new Command(() => AddMeterOfPole(), () => true);
            UpdateCommand = new Command(async () => await Update(), () => true);
        }

        private async Task Update()
        {
            var list = new List<MeterOfPoleDto>();
            foreach (var item in this.MetersOfPole)
            {
                list.Add(new MeterOfPoleDto { meterSerialId = item.MeterId });
            }
            var dto = new EnergyMeterCreateDto(this.SerialId, list, this.SelectType);
            var response = await Service.Update(dto);
            if (response.IsSuccessStatusCode)
            {
                var meters = await Service.GetAll();
                await Navigation.PushAsync(new MainView(new MainViewModel(Service, meters)));
            }
        }

        private void AddMeterOfPole()
        {
            if (SelectType.Equals("Pole"))
            {
                MetersOfPole.Add(new MeterOfPoleViewModel { MeterId = "" });
            }
            else
            {
                MetersOfPole = new ObservableCollection<MeterOfPoleViewModel>();
            }
        }

        public List<string> TypesMeter
        {
            get
            {
                return new List<string> { "House", "Pole" };
            }
        }

        private string _SelectType { get; set; }

        public string SelectType
        {
            get { return _SelectType; }
            set
            {
                if (_SelectType != value)
                {
                    _SelectType = value;
                    RaisePropertyChanged(nameof(SelectType));
                    if (SelectType.Equals("Pole"))
                    {
                        IsPole = true;
                    }
                    else
                    {
                        IsPole = false;
                    }
                }
            }
        }

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

        private bool _IsPole { get; set; }

        public bool IsPole
        {
            get { return _IsPole; }
            set
            {
                if (_IsPole != value)
                {
                    _IsPole = value;
                    RaisePropertyChanged(nameof(IsPole));
                }
            }
        }

        public ICommand UpdateCommand { get; set; }
        public ICommand AddMeterOfPoleCommand { get; set; }
        private TypeOfEnergyMeter _Type { get; set; }
        public ObservableCollection<MeterOfPoleViewModel> MetersOfPole { get; set; } = new ObservableCollection<MeterOfPoleViewModel>();
    }
}