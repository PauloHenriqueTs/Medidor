using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class CountAmrCommand : IAmrCommand
    {
        public CountAmrCommand()
        {
            Type = AmrCommandType.GetCount;
        }
    }
}