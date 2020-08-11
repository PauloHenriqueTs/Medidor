using App1.Dto.ValueObject;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace App1.Dto
{
    public class EnergyMeter
    {
        [Newtonsoft.Json.JsonConstructor]
        public EnergyMeter(string serialId, string userId, TypeOfEnergyMeter type, List<MeterOfPole> meters, string count, bool switchState)
        {
            SerialId = serialId;
            UserId = userId;
            Type = type;
            if ( meters ==null|| !meters.Any() )
            {
                Meters = new List<MeterOfPole> { new MeterOfPole { MeterId="-1"} };
            }
            else
            {
                Meters = meters;
            }

            Count = count;
            SwitchState = switchState;
        }

        public string SerialId { get; set; }

        public string UserId { get; set; }

        public TypeOfEnergyMeter Type { get; set; }

        public List<MeterOfPole> Meters { get; set; } = new List<MeterOfPole>();

        public string Count { get; set; }

        public bool SwitchState { get; set; }
    }
}