using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Models
{
    //An area with open playing cards. Responsible for processing moves
    public class GameBoard
    {
        private Dictionary<Player, List<Card>> openCards;

        public GameBoard(Player playerA, Player playerB)
        {
            openCards = new Dictionary<Player, List<Card>>
            {
                { playerA, new List<Card>() },
                { playerB, new List<Card>() }
            };
            playerA.MakeMove += ToProcessMove;
            playerB.MakeMove += ToProcessMove;
        }

        public IReadOnlyDictionary<Player, List<Card>> OpenCards
        {
            get
            {
                return openCards;
            }
        }

        private void ToProcessMove(object sender, MakeMoveEventArgs args)
        {
            if (sender is Player player && openCards.ContainsKey(player))
            {
                Card card = args.Card;
                openCards[player].Add(card);
                card.Destroy += ToProcessDestructionCard;
            }
        }

        private void ToProcessDestructionCard(object sender, EventArgs args)
        {
            if (sender is Card card)
            {
                foreach (Player player in openCards.Keys)
                {
                    if (openCards[player].Remove(card))
                    {
                        card.Destroy -= ToProcessDestructionCard;
                        break;
                    }
                }
            }
        }

        public int GetGamePoints(Player player)
        {
            int sum = openCards[player].Sum(c => c.GamePoints);
            return sum;
        }
    }
}