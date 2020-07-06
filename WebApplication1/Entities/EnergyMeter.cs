using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class EnergyMeter
    {
        [Key]
        public int serialId { get; private set; }

        public ApplicationUser user { get; set; }

        public string userId { get; set; }

        [AllowNull]
        private List<MeterOfPole> _EnergyMeters { get; set; }

        [AllowNull]
        [NotMapped]
        public ReadOnlyCollection<MeterOfPole> meterOfPoleEnergyMeter { get { return _EnergyMeters.AsReadOnly(); } }
    }
}