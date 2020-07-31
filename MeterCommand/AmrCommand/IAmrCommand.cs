using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public abstract class IAmrCommand
    {
        protected IAmrCommand(AmrCommandType type)
        {
            Type = type;
        }

        public AmrCommandType Type { get; set; }
    }
}