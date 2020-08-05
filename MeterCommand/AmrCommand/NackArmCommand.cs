using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class NackArmCommand : IAmrCommand
    {
        public NackArmCommand(string serialId)
        {
            this.serialId = serialId;
            Type = AmrCommandType.NACK;
        }

        public string serialId { get; set; }
    }
}