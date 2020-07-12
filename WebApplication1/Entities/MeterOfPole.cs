using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class MeterOfPole
    {
        public MeterOfPole(string meterId)
        {
            MeterId = meterId;
        }

        [Required]
        public string MeterId { get; private set; }

        public override bool Equals(object obj)
        {
            return obj is MeterOfPole pole &&
                   MeterId == pole.MeterId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(MeterId);
        }
    }
}