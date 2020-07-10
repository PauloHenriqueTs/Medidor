﻿using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ValueObjects;
using WebApplication1.ViewModel;

#nullable enable

namespace WebApplication1.Entities
{
    public class EnergyMeter
    {
        public EnergyMeter(string serialId, string userId, TypeOfEnergyMeter type, List<MeterOfPole> meters)
        {
            SerialId = serialId;
            UserId = userId;
            Type = type;
            Meters = meters;
        }

        public string SerialId { get; private set; }

        public string UserId { get; private set; }

        public TypeOfEnergyMeter Type { get; private set; }

        public List<MeterOfPole>? Meters { get; private set; }
    }
}