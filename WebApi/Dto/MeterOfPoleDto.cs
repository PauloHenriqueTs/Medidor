using System.ComponentModel.DataAnnotations;

namespace WebApi.Dto
{
    public class MeterOfPoleDto
    {
        [Required]
        public string meterSerialId { get; set; }
    }
}