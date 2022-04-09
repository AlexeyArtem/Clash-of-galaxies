using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public delegate void EndRoundEventHandler(object sender, EndRoundEventArgs args);
    
    public class EndRoundEventArgs : EventArgs
    {
        public EndRoundEventArgs(Player winPlayer) 
        {
            WinPlayer = winPlayer;
        }
        public Player WinPlayer { get; }
    }
}
