using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogisticsSystem.Domain.Enums;
using LogisticsSystem.Domain.Models.Identity;

namespace LogisticsSystem.Domain.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public Guid TrackingId { get; set; } = Guid.NewGuid();
        public string SenderName { get; set; } = null!;
        [Phone]
        public string SenderPhone { get; set; } = null!;
        public string ReceiverName { get; set; } = null!;
        public string ReceiverPhone { get; set; } = null!;
        public ShipmentStatus Status { get; set; }
        public string PackType { get; set; } = null!;
        public decimal Weight { get; set; }
        public string DeliveryAddress { get; set; } = null!;
        public string? ProofImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        //Foreign Keys
        public int? WarehouseId { get; set; }
        public int CustomerId { get; set; }
        public int? DeliveryAgentId { get; set; }

        public Warehouse? Warehouse { get; set; }
        public ApplicationUser Customer { get; set; } = null!;
        public ApplicationUser? DeliveryAgent { get; set; }
        public ICollection<ShipmentTrackingHistory> TrackingHistories { get; set; } = new HashSet<ShipmentTrackingHistory>();
    }
}
