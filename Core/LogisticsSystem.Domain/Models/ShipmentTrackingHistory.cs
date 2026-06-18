using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticsSystem.Domain.Enums;

namespace LogisticsSystem.Domain.Models
{
    public class ShipmentTrackingHistory
    {
        public int Id { get; set; }
        public ShipmentStatus Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        //Foreign Key
        public int ShipmentId { get; set; }
    }
}
