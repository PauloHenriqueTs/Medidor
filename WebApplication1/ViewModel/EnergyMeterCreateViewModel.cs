using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            this.serialId = 3324343;
            this.select = "";

            this.EnergyTypeList = new SelectList(
          new List<SelectListItem>
          {
            new SelectListItem {Text = "At home", Value = "House"},
            new SelectListItem {Text = "On the pole", Value = "Pole"},
          }, "Value", "Text");
        }

        [Required]
        public int serialId { get; set; }

        [Required]
        public SelectList EnergyTypeList { get; set; }

        public List<MeterOfPole> meterOfPoles { get; set; }

        [DisplayName("Select Type")]
        public string select { get; set; }
    }
}