using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class MakeMoveEventArgs : EventArgs
    {
        public MakeMoveEventArgs(Card card) : base()
        {
            Card = card;
        }

        public Card Card { get; }
    }

    public delegate void MakeMoveEventHandler(object sender, MakeMoveEventArgs args);
}