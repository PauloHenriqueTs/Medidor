using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data.DAO
{
    public class MeterOfPoleEnergyMeter
    {
        public MeterOfPoleEnergyMeter()
        {
        }

        public MeterOfPoleEnergyMeter(string meterId, string poleEnergyMeterId, PoleEnergyMeter poleEnergyMeter)
        {
            MeterId = meterId;
            PoleEnergyMeterId = poleEnergyMeterId;
            PoleEnergyMeter = poleEnergyMeter;
        }

        [Key]
        public string MeterId { get; set; }

        public string PoleEnergyMeterId { get; set; }

        public PoleEnergyMeter PoleEnergyMeter { get; set; }
    }
}