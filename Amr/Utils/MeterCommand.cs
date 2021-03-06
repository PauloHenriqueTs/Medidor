﻿using Amr.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amr.Utils
{
    public class MeterCommand
    {
        public HouseMeter value { get; set; }
        public MeterCommandType type { get; set; }
    }

    public enum MeterCommandType
    {
        Switch,
        Count,
        Ack,
        Syn
    }
}