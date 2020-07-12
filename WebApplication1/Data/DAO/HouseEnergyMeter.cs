using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Entities;
using WebApplication1.ValueObjects;

namespace WebApplication1.Data.DAO
{
    public class HouseEnergyMeter
    {
        public HouseEnergyMeter(string serialId, string userId)
        {
            SerialId = serialId;
            UserId = userId;
        }

        public HouseEnergyMeter()
        {
        }

        public EnergyMeter ToEnergyMeter()
        {
            return new EnergyMeter(this.SerialId, this.UserId, TypeOfEnergyMeter.House, null);
        }

        [Key]
        public string SerialId { get; set; }


        public string UserId { get; set; }
    }
}