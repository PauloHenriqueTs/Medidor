using Entities.ValueObjects;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace Entities
{
    public class EnergyMeter
    {
        public EnergyMeter(string serialId, string userId, TypeOfEnergyMeter type, List<MeterOfPole>? meters, string Count, bool SwitchState)
        {
            SerialId = serialId;
            UserId = userId;
            Type = type;
            this.Count = Count;
            this.SwitchState = SwitchState;
            if (type == TypeOfEnergyMeter.Pole && meters != null)
            {
                var removeDuplicate = meters.Distinct().ToList();
                Meters = removeDuplicate;
            }
        }

        public string SerialId { get; private set; }

        public string UserId { get; private set; }

        public TypeOfEnergyMeter Type { get; private set; }

        public List<MeterOfPole>? Meters { get; private set; }

        public string Count { get; set; }

        public bool SwitchState { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is EnergyMeter meter &&
                   SerialId == meter.SerialId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SerialId, UserId, Type, Meters);
        }
    }
}