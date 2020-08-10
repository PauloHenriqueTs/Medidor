using App1.Dto.ValueObject;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace App1.Dto
{
    public class EnergyMeter
    {
        public string SerialId { get; set; }

        public string UserId { get; set; }

        public TypeOfEnergyMeter Type { get; set; }

        [DefaultValue("[]")]
        public List<MeterOfPole> Meters { get; set; } = new List<MeterOfPole>();

        public string Count { get; set; }

        public bool SwitchState { get; set; }
    }
}