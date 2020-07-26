using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Command
{
    public class MeterCommand : IMeterCommand
    {
        public MeterCommand(MeterCommandType type, string serialId, string userId) : base(type)
        {
            SerialId = serialId;
            UserId = userId;
            CreateAt = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MeterCommandId { get; set; }

        public string SerialId { get; private set; }

        public DateTime CreateAt { get; private set; }

        public string UserId { get; private set; }
    }
}