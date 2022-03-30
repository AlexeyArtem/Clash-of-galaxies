using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    public class DealCardsEventArgs : EventArgs
    {
        public DealCardsEventArgs(Player player, List<Card> cards)
        {
            Player = player;
            Cards = cards;
        }

        public Player Player { get; }
        public List<Card> Cards { get; }
    }

    public delegate void DealCardsEventHandler(object sender, DealCardsEventArgs args);
}
