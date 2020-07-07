using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModel;

namespace WebApplication1.Entities
{
    public class EnergyMeter
    {
        public EnergyMeter()
        {
        }

        public static EnergyMeter Create(ApplicationUser u, EnergyMeterCreateViewModel model)
        {
            var energymeter = new EnergyMeter();
            energymeter.serialId = model.serialId;
            energymeter.user = u;
            energymeter.userId = u.Id;
            energymeter.Type = model.Select;
            if (model.Select == "Pole")
            {
                energymeter.EnergyMeters = new List<MeterOfPole>();
                foreach (var item in model.meterOfPoles)
                {
                    energymeter.EnergyMeters.Add(MeterOfPole.Create(model.serialId, item.meterSerialId, energymeter));
                }
            }
            return energymeter;
        }

        [Key]
        public int serialId { get; private set; }

        public ApplicationUser user { get; set; }

        public string userId { get; set; }

        public string Type { get; set; }

        [AllowNull]
        [Column]
        public List<MeterOfPole> EnergyMeters { get; set; }

        
    }
}