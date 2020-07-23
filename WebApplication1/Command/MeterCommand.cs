using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Command
{
    public class MeterCommand
    {
        public HouseMeter value { get; set; }
        public MeterCommandType type { get; set; }
    }

    public enum MeterCommandType
    {
        Switch,
        Count
    }

    public class HouseMeter
    {
        public bool Switch { get; set; } = true;

        public string serialId
        {
            get; set;
        }

        public string count
        {
            get; set;
        }
    }
}