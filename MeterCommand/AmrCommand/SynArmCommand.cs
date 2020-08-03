using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class SynArmCommand : IAmrCommand
    {
        public AmrMeter Meter { get; set; }

        public SynArmCommand(AmrMeter meter)
        {
            Meter = meter;
            Type = AmrCommandType.SYN;
        }
    }
}