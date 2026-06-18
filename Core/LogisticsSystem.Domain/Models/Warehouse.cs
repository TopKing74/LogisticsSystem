using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Domain.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int capacity { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new HashSet<Shipment>();
        public int CurrentOccupancy => Shipments?.Count ?? 0;
    }
}
