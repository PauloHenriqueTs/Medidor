using Entities;
using Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.DAO
{
    internal class PoleEnergyMeter
    {
        public PoleEnergyMeter(string serialId, string userId, List<MeterOfPole> meterOfPoles)
        {
            SerialId = serialId;
            UserId = userId;
            MeterOfPoleEnergyMeters = new List<MeterOfPoleEnergyMeter>();
            foreach (var item in meterOfPoles)
            {
                MeterOfPoleEnergyMeters.Add(new MeterOfPoleEnergyMeter(item.MeterId, serialId, this));
            }
        }

        public PoleEnergyMeter()
        {
        }

        public EnergyMeter ToEnergyMeter()
        {
            var list = new List<MeterOfPole>();
            foreach (var item in MeterOfPoleEnergyMeters)
            {
                list.Add(new MeterOfPole(item.MeterId));
            }

            return new EnergyMeter(SerialId, UserId, TypeOfEnergyMeter.Pole, list, "0", true);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PoleEnergyMeterId { get; set; }

        public string SerialId { get; set; }

        public string UserId { get; set; }

        [Required]
        public List<MeterOfPoleEnergyMeter> MeterOfPoleEnergyMeters { get; set; }
    }
}