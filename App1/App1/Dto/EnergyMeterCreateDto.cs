using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Dto
{
    public class EnergyMeterCreateDto
    {
        public EnergyMeterCreateDto(string serialId, List<MeterOfPoleDto> meterOfPoles, string Select)
        {
            this.serialId = serialId;
            this.meterOfPoles = meterOfPoles;
            select = Select;
        }

        public string serialId { get; set; }

        public List<MeterOfPoleDto> meterOfPoles { get; set; }

        public string select { get; set; }
    }
}