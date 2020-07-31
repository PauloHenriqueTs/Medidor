using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class AckArmCommand : IAmrCommand
    {
        public AckArmCommand(string serialId, string port, AmrCommandType type) : base(type)
        {
            this.serialId = serialId;

            this.port = port;
        }

        public string serialId { get; set; }

        public string port { get; set; }
    }
}