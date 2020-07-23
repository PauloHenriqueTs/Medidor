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
    internal class HouseEnergyMeter
    {
        public HouseEnergyMeter(string serialId, string userId, string count, bool switchState)
        {
            SerialId = serialId;
            UserId = userId;
            Count = count;
            SwitchState = switchState;
        }

        public HouseEnergyMeter()
        {
        }

        public EnergyMeter ToEnergyMeter()
        {
            return new EnergyMeter(SerialId, UserId, TypeOfEnergyMeter.House, null, Count, SwitchState);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HouseEnergyMeterId { get; set; }

        public string SerialId { get; set; }

        public string Count { get; set; }

        public bool SwitchState { get; set; }
        public string UserId { get; set; }
    }
}