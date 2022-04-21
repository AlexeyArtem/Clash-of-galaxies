using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public delegate void EndEventHandler(object sender, EndEventArgs args);
    
    public class EndEventArgs : EventArgs
    {
        public EndEventArgs(Player winPlayer) 
        {
            WinPlayer = winPlayer;
        }
        public Player WinPlayer { get; }
    }
}
