using PokerBotHost.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PokerBotHost.Models
{
    public class Player
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public Guid Token { get; set; }
        
        public long TableId { get; set; }
        public PokerTable Table { get; set; }

        public string HoleCards { get; set; }
        public bool isDealer { get; set; }
        public int chipCount { get; set; }
        public int chipsInPlay { get; set; }
    }
}
