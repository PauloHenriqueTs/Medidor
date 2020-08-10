using System;
using System.Collections.Generic;
using System.Text;

namespace App1.ViewModel
{
    public class MyTestViewModel : ViewModel
    {
        private string name { get; set; } = "Deu Bom o Test";

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
    }
}