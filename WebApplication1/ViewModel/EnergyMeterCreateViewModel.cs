using Microsoft.AspNetCore.Mvc;
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
            ResetSelectList();
        }

        public void ResetSelectList()
        {
            EnergyTypeList = new SelectList(
         new List<SelectListItem>
         {
            new SelectListItem {Text = "At home", Value = "House"},
            new SelectListItem {Text = "On the pole", Value = "Pole"},
         }, "Value", "Text");
        }

        [Required]
        public string serialId { get; set; }

        [Required]
        public SelectList EnergyTypeList { get; set; }

        
        public List<MeterOfPoleDto> meterOfPoles { get; set; }

        [DisplayName("Select Type")]
        public string Select { get; set; }
    }

    public class MeterOfPoleDto
    {
        public string meterSerialId { get; set; }
    }
}