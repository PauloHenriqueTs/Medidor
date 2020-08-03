using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class AckArmCommand : IAmrCommand
    {
        public AckArmCommand(AmrMeter meter, string port)
        {
            Meter = meter;
            this.port = port;
            Type = AmrCommandType.ACK;
        }

        public AmrMeter Meter { get; set; }
        public string port { get; set; }
    }
}