﻿using Newtonsoft.Json;
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

        public List<Player> Players { get; set; }

        public string flopCards { get; set; }
        public string turnCard { get; set; }
        public string riverCard { get; set; }

        public int potSize { get; set; }

    }
}
