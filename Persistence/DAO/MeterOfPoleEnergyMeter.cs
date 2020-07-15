using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.DAO
{
    internal class MeterOfPoleEnergyMeter
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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeterOfPoleEnergyMeterId { get; set; }

        public string MeterId { get; set; }

        public string PoleEnergyMeterId { get; set; }

        public PoleEnergyMeter PoleEnergyMeter { get; set; }
    }
}