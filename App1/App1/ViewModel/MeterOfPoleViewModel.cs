using System;
using System.Collections.Generic;
using System.Text;

namespace App1.ViewModel
{
    public class MeterOfPoleViewModel : ViewModel
    {
        private string _MeterId { get; set; }

        public string MeterId
        {
            get { return _MeterId; }
            set
            {
                if (_MeterId != value)
                {
                    _MeterId = value;
                    RaisePropertyChanged(nameof(MeterId));
                }
            }
        }
    }
}