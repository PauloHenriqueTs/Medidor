using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public abstract class IMeterCommand
    {
        protected IMeterCommand(MeterCommandType type)
        {
            Type = type;
        }

        public MeterCommandType Type { get; private set; }
    }
}