using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Persistence.DAO
{
    internal class MeterOfPoleDao
    {
        

        public MeterOfPoleDao()
        {
        }

        public MeterOfPoleDao(EnergyMeter meter, string meterId)
        {
            MeterOfPoleDaoId = meterId;
            EnergyMeterDaoId = meter.SerialId;
        }

        [Key]
        public string MeterOfPoleDaoId { get; set; }

        public string EnergyMeterDaoId { get; set; }

        public override bool Equals(object obj)
        {
            return obj is MeterOfPoleDao pole &&
                   MeterOfPoleDaoId == pole.MeterOfPoleDaoId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MeterOfPoleDaoId);
        }

        internal MeterOfPole ToMeterOfPole()
        {
            return new MeterOfPole(MeterOfPoleDaoId);
        }
    }
}