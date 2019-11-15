using System;
using System.Collections.Generic;

using Yugioh.Engine.Entities;
using Yugioh.Engine.Exceptions;

namespace Yugioh.Engine.Models
{
    public class Save
    {     
        public DateTime Timestamp { get; set; }
        public string PlayerName { get; set; }
        public int PlayerDuelistPoints { get; set; }
        public IList<string> PlayerCardNumbers { get; set; }
        public string PlayerActiveDeckName { get; set; }

        public Save()
        {
          this.Timestamp = DateTime.Now;
          this.PlayerCardNumbers = new List<string>();
        }
    }
}
