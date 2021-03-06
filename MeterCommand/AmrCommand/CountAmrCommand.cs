﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class CountAmrCommand : IAmrCommand
    {
        public CountAmrCommand(string serialId)
        {
            this.serialId = serialId;
            Type = AmrCommandType.GetCount;
        }

        public string serialId { get; set; }
    }
}