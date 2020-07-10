using System;
using System.Collections.Generic;
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

        public string MeterId { get; private set; }
    }
}