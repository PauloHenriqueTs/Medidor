using Entities;
using Entities.ValueObjects;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

#nullable enable

namespace Persistence.DAO
{
    internal class EnergyMeterDao
    {
        public EnergyMeterDao()
        {
        }

        public EnergyMeterDao(EnergyMeter meter)
        {
            MeterOfPoleDao = null;
            if (meter.Meters != null)
            {
                var dao = new List<MeterOfPoleDao>();
                foreach (var item in meter.Meters)
                {
                    dao.Add(new MeterOfPoleDao(meter, item.MeterId));
                }

                MeterOfPoleDao = dao;
            }

            SerialId = meter.SerialId;
            UserId = meter.UserId;
            Type = meter.Type;
            Count = meter.Count;
            SwitchState = meter.SwitchState;
        }

        public EnergyMeter ToEnergyMeter()
        {
            if (MeterOfPoleDao != null)
            {
                var meterOfPoles = new List<MeterOfPole>();
                foreach (var item in MeterOfPoleDao)
                {
                    meterOfPoles.Add(item.ToMeterOfPole());
                }

                return new EnergyMeter(SerialId, UserId, Type, meterOfPoles, Count, SwitchState);
            }

            return new EnergyMeter(SerialId, UserId, Type, null, Count, SwitchState);
        }

        [Key]
        public string SerialId { get; set; }

        public string UserId { get; set; }

        public TypeOfEnergyMeter Type { get; set; }

        public List<MeterOfPoleDao>? MeterOfPoleDao { get; set; }

        public string Count { get; set; }

        public bool SwitchState { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is EnergyMeterDao meter &&
                   SerialId == meter.SerialId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SerialId, UserId, Type, MeterOfPoleDao);
        }
    }
}