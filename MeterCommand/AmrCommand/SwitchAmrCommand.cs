using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class SwitchAmrCommand : IAmrCommand
    {
        public SwitchAmrCommand(string serialId)
        {
            this.serialId = serialId;
            Type = AmrCommandType.Switch;
        }

        public string serialId { get; set; }
    }
}