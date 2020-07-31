using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class SynArmCommand : IAmrCommand
    {
        public string serialId { get; set; }

        public string ip { get; set; }
        public string port { get; set; }

        public SynArmCommand(string serialId, string ip, string port, AmrCommandType type) : base(type)
        {
            this.serialId = serialId;
            this.ip = ip;
            this.port = port;
        }
    }
}