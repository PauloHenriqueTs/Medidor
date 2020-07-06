using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class MeterOfPole
    {
        public int poleSerialId { get; private set; }

        [Key]
        public int meterSerialId { get; private set; }

        public EnergyMeter poleMeter { get; private set; }
    }
}