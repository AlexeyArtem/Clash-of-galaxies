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

        public MakeMoveEventArgs(Player player, Card card) : this(card)
        {
            Player = player;
        }

        public MakeMoveEventArgs(Card card, Card targetCard) : this(card)
        {
            TargetCard = targetCard;
        }

        public Card Card { get; }
        public Card TargetCard { get; }
        public Player Player { get; }
    }

    public delegate void MakeMoveEventHandler(object sender, MakeMoveEventArgs args);
}