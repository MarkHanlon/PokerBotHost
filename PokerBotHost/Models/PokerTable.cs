using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerBotHost.Models
{
    public enum TableStates
    {
        Registering,
        Playing,
        Closed
    }

    public class PokerTable
    {
        public long Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TableStates TableState { get; set; } 


    }
}
