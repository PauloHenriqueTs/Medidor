﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public abstract class IAmrCommand
    {
        public AmrCommandType Type { get; set; }
    }
}