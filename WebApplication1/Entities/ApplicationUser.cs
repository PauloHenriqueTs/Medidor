using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            _EnergyMeters = new List<EnergyMeter>();
        }

        [AllowNull]
        private List<EnergyMeter> _EnergyMeters { get; set; }

        [AllowNull]
        [NotMapped]
        public ReadOnlyCollection<EnergyMeter> EnergyMeters { get { return _EnergyMeters.AsReadOnly(); } }
    }
}