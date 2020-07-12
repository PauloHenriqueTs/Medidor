using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.ValueObjects;

namespace WebApplication1.Data.DAO
{
    public class PoleEnergyMeter
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

            return new EnergyMeter(this.SerialId, this.UserId, TypeOfEnergyMeter.Pole, list);
        }

        [Key]
        public string SerialId { get; set; }


        public string UserId { get; set; }

        public List<MeterOfPoleEnergyMeter> MeterOfPoleEnergyMeters { get; set; }
    }
}