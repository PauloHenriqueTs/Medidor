using App1.Dto.ValueObject;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App1.Dto
{
    public class EnergyMeter
    {
        [JsonConstructor]
        public EnergyMeter(string serialId, string userId, TypeOfEnergyMeter type, List<MeterOfPole> meters, string count, bool switchState)
        {
            SerialId = serialId;
            UserId = userId;
            Type = type;
            if(Meters == null) {
                Meters = new List<MeterOfPole>();
            }else
            {
                Meters = meters; 
            }
 
            Count = count;
            SwitchState = switchState;
        }

        public string SerialId { get; set; }

        public string UserId { get; set; }

        public TypeOfEnergyMeter Type { get; set; }

       
        public List<MeterOfPole> Meters { get; set; }

        public string Count { get; set; }

        public bool SwitchState { get; set; }
    }
}