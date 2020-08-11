using System;
using System.Collections.Generic;
using System.Text;

namespace Command
{
    public enum MeterCommandType
    {
        CreateMeterCommand,
        UpdateMeterCommand,
        DeleteMeterCommand,
        GetCountCommand,
        SwitchCommand
    }
}