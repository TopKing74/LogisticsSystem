using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Domain.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        //Foreign Key
        public int UserId { get; set; }
    }
}
