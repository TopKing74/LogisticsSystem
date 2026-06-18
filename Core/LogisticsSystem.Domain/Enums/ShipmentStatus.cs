using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Domain.Enums
{
    public enum ShipmentStatus
    {
        Created,
        PickedUp,
        InWarehouse,
        OutForDelivery,
        Delivered
    }
}
