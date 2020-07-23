using Entities;
using Entities.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.ViewModel
{
    public class EnergyMeterUpdateViewModel
    {
        public EnergyMeterUpdateViewModel(EnergyMeter energyMeter)
        {
            ResetSelectList();
            serialId = energyMeter.SerialId;
            Select = energyMeter.Type.ToString();
            Count = energyMeter.Count;
            Switch = energyMeter.SwitchState;
            meterOfPoles = new List<MeterOfPoleDto>();
            if (energyMeter.Meters != null && energyMeter.Meters.Any())
            {
                foreach (var item in energyMeter.Meters)
                {
                    meterOfPoles.Add(new MeterOfPoleDto { meterSerialId = item.MeterId });
                }
            }
            meterOfPoles.Add(new MeterOfPoleDto { meterSerialId = "0" });
        }

        public EnergyMeterUpdateViewModel()
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

        public EnergyMeter toEnergyMeter(string userId)
        {
            if (Select == "House")
            {
                return new EnergyMeter(serialId.ToString(), userId, TypeOfEnergyMeter.House, null, Count, Switch);
            }
            else if (Select == "Pole" && meterOfPoles.Any())
            {
                var meters = new List<MeterOfPole>();
                foreach (var item in meterOfPoles)
                {
                    meters.Add(new MeterOfPole(item.meterSerialId));
                }
                return new EnergyMeter(serialId.ToString(), userId, TypeOfEnergyMeter.Pole, meters, Count, Switch);
            }
            return null;
        }

        [Required]
        public string serialId { get; set; }

        private string Count { get; set; }

        private bool Switch { get; set; }

        [Required]
        public SelectList EnergyTypeList { get; set; }

        public List<MeterOfPoleDto> meterOfPoles { get; set; }

        [DisplayName("Select Type")]
        [Required]
        public string Select { get; set; }
    }
}