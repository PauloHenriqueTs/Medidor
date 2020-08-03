using System;
using System.Collections.Generic;
using System.Text;

namespace Command.AmrCommand
{
    public class AmrMeter
    {
        public AmrMeter(string serialId, string count, bool @switch)
        {
            this.serialId = serialId;
            this.count = count;
            Switch = @switch;
        }

        public string serialId { get; set; }
        public string count { get; set; }

        public bool Switch { get; set; }
    }
}