using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class MeterOfPole
    {
        public MeterOfPole()
        {
            this.poleSerialId = poleSerialId;
            this.meterSerialId = meterSerialId;
            this.poleMeter = poleMeter;
        }

        public static MeterOfPole Create(int poleSerialId, int meterSerialId, EnergyMeter poleMeter)
        {
            var meter = new MeterOfPole();
            meter.poleSerialId = poleSerialId;
            meter.meterSerialId = meterSerialId;
            meter.poleMeter = poleMeter;

            return meter;
        }

        public int poleSerialId { get; private set; }

        [Key]
        public int meterSerialId { get; private set; }

        public EnergyMeter poleMeter { get; private set; }
    }
}