using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LogisticsSystem.Domain.Models.Identity
{
    public class ApplicationUser:IdentityUser<int>
    {
        public string DisplayName { get; set; } = null!;
        public Address Address { get; set; }

        public ICollection<Shipment> CreatedShipments { get; set; } = new HashSet<Shipment>();
        public ICollection<Shipment> DeliveredShipments { get; set; } = new HashSet<Shipment>();
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    }
}
