using Entities;
using Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dto
{
    public class EnergyMeterCreateDto
    {
        public EnergyMeter toEnergyMeter(string userId)
        {
            if (Select == "House")
            {
                return new EnergyMeter(serialId, userId, TypeOfEnergyMeter.House, null, "0", true);
            }
            else if (Select == "Pole" && meterOfPoles.Any())
            {
                var meters = new List<MeterOfPole>();
                foreach (var item in meterOfPoles)
                {
                    meters.Add(new MeterOfPole(item.meterSerialId));
                }
                return new EnergyMeter(serialId, userId, TypeOfEnergyMeter.Pole, meters, "0", true);
            }
            return null;
        }

        [Required]
        public string serialId { get; set; }

        public List<MeterOfPoleDto> meterOfPoles { get; set; }

        [Required]
        public string Select { get; set; }
    }
}