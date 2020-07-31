using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Amr.Model
{
    public class HouseMeter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string nomePropriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nomePropriedade));
        }

        private string _serialId;
        private string _count;
        private bool _connect;

        public bool connect
        {
            get { return _connect; }
            set
            {
                _connect = value;
                OnPropertyChanged(nameof(connect));
            }
        }

        public bool _Switch { get; set; } = true;

        public bool Switch
        {
            get { return _Switch; }
            set
            {
                _Switch = value;
                OnPropertyChanged(nameof(Switch));
            }
        }

        public string ip { get; set; } = "";

        public string serialId
        {
            get { return _serialId; }
            set
            {
                _serialId = value;
                OnPropertyChanged(nameof(serialId));
            }
        }

        public string count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged(nameof(count));
            }
        }
    }
}