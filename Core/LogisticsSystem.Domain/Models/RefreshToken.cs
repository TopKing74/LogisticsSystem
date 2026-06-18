using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsSystem.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
    }
}
