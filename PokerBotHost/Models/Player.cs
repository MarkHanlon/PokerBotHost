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
        
        public string HoleCard1 { get; set;}
        //public Card HoleCard2 { get; set; }
        public long TableId { get; set; }
    }
}
