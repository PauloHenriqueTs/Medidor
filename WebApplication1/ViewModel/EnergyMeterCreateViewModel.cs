using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;

namespace WebApplication1.ViewModel
{
    public class EnergyMeterCreateViewModel
    {
        public EnergyMeterCreateViewModel()
        {
        }

        [Required]
        public int serialId { get; set; }

        public EnergyMeterType energyMeterType { get; set; }
    }
}